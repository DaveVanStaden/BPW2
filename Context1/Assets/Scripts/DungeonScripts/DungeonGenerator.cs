using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Example
{
    public class DungeonGenerator: MonoBehaviour
    {
        public enum Tile { Floor }

        [Header("Prefabs")]
        public GameObject FloorPrefab;
        public GameObject CorridorFloorPrefab;
        public GameObject WallPrefab;
        public GameObject Player;
        public GameObject Enemy;
        public GameObject WallContainer;
        public GameObject FloorContainer;


        [Header("Dungeon Settings")]
        public int amountRooms;
        public int width = 100;
        public int height = 100;
        public int minRoomSize = 3;
        public int maxRoomSize = 7;

        private Dictionary<Vector2Int, Tile> dungeonDictionary = new Dictionary<Vector2Int, Tile>();
        private List<Room> roomList = new List<Room>();
        private List<GameObject> allSpawnedObjects = new List<GameObject>();

        [Header("Other Variables")]
        private bool playerSpawned;
        private int roomCount;
        private int WaitBeforeSpawn;
        public int MobCount = 3;
        public NavMeshSurface surface;
        void Start()
        {
            GenerateDungeon();
            surface.BuildNavMesh();
        }

        private void AllocateRooms()
        {
            for (int i = 0; i < amountRooms; i++)
            {
                Room room = new Room()
                {
                    position = new Vector2Int(Random.Range(0, width), Random.Range(0, height)),
                    size = new Vector2Int(Random.Range(minRoomSize, maxRoomSize), Random.Range(minRoomSize, maxRoomSize))
                };

                if (CheckIfRoomFitsInDungeon(room))
                {
                    AddRoomToDungeon(room);
                }
                else
                {
                    i--;
                }
            }
        }

        private void AddRoomToDungeon(Room room)
        {
            for (int xx = room.position.x; xx < room.position.x + room.size.x; xx++)
            {
                for (int yy = room.position.y; yy < room.position.y + room.size.y; yy++)
                {
                    Vector2Int pos = new Vector2Int(xx, yy);
                    dungeonDictionary.Add(pos, Tile.Floor);
                    
                    if(roomCount >= 1)
                        {
                        int rand = Random.Range(0, 80);
                        if (rand <= 3 && MobCount != 0 && WaitBeforeSpawn <=0)
                        {
                            Instantiate(Enemy, new Vector3(xx, 0.5f, yy), Quaternion.identity);
                            MobCount--;
                            WaitBeforeSpawn = 55;
                        }
                        WaitBeforeSpawn -= 1;
                    }
                    
                }
            }
            roomCount += 1;
            roomList.Add(room);
            MobCount = 3;
        }

        private bool CheckIfRoomFitsInDungeon(Room room)
        {
            for (int xx = room.position.x; xx < room.position.x + room.size.x; xx++)
            {
                for (int yy = room.position.y; yy < room.position.y + room.size.y; yy++)
                {
                    Vector2Int pos = new Vector2Int(xx, yy);
                    if (dungeonDictionary.ContainsKey(pos))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        private void AllocateCorridors()
        {

            for (int i = 0; i < roomList.Count; i++)
            {
                //modulo
                //10 % 3 = 1
                //20 % 3 = 2
                // 0 1 2 3 4 5 [6 7]
                Room startRoom = roomList[i];
                Room otherRoom = roomList[(i + Random.Range(1, roomList.Count - 1)) % roomList.Count];

                // -1, 0, 1
                int dirX = Mathf.RoundToInt(Mathf.Sign(otherRoom.position.x - startRoom.position.x));
                for(int x = startRoom.position.x; x != otherRoom.position.x; x += dirX)
                {
                    Vector2Int pos = new Vector2Int(x, startRoom.position.y);
                    if (playerSpawned == false)
                    {
                        Instantiate(Player, new Vector3(x,1,startRoom.position.y), Quaternion.identity);
                        playerSpawned = true;
                    }
                    if (!dungeonDictionary.ContainsKey(pos))
                    {
                        dungeonDictionary.Add(pos, Tile.Floor);
                    }
                }

                int dirY = Mathf.RoundToInt(Mathf.Sign(otherRoom.position.y - startRoom.position.y));
                for (int y = startRoom.position.y; y != otherRoom.position.y; y += dirY)
                {
                    Vector2Int pos = new Vector2Int(otherRoom.position.x, y);
                    if (!dungeonDictionary.ContainsKey(pos))
                    {
                        dungeonDictionary.Add(pos, Tile.Floor);
                    }
                }

            }



        }

        private void BuildDungeon()
        {
            foreach(KeyValuePair<Vector2Int, Tile> kv in dungeonDictionary)
            {
                GameObject floor = Instantiate(FloorPrefab, new Vector3Int(kv.Key.x, 0, kv.Key.y), Quaternion.identity);
                allSpawnedObjects.Add(floor);
                floor.transform.SetParent(FloorContainer.transform);

                SpawnWallsForTile(kv.Key);
            }
        }

        private void SpawnWallsForTile(Vector2Int position)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if(Mathf.Abs(x) == Mathf.Abs(z)) { continue; }
                    Vector2Int gridPos = position + new Vector2Int(x, z);
                    if (!dungeonDictionary.ContainsKey(gridPos))
                    {
                        //Spawn Wall
                        Vector3 direction = new Vector3(gridPos.x, 0, gridPos.y) - new Vector3(position.x, 0, position.y);
                        GameObject wall = Instantiate(WallPrefab, new Vector3(position.x, 0, position.y), Quaternion.LookRotation(direction));
                        allSpawnedObjects.Add(wall);
                        wall.transform.SetParent(WallContainer.transform);
                    }
                }
            }
        }


        public void GenerateDungeon()
        {
            AllocateRooms();
            AllocateCorridors();
            BuildDungeon();
        }

    }

    public class Room
    {
        public Vector2Int position;
        public Vector2Int size;
    }
}
