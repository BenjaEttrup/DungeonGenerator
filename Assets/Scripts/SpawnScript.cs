using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
	public GameObject[] rooms;
	public GameObject[] endRooms;
	public int maxRooms = 100;

	private List<Exit> currentExits = new List<Exit>();
	private List<GameObject> map = new List<GameObject>();
	private GameObject[] availRooms;
	private GameObject firstRoom;

	public GameObject Path;

	private void Start()
    {
        
        if (!(map.Count >= maxRooms))
        {
			int rndSpawnRoom = UnityEngine.Random.Range(0, endRooms.Length);

			firstRoom = Instantiate(endRooms[rndSpawnRoom], this.gameObject.transform.position, Quaternion.identity);

			firstRoom.GetComponent<Room>().isStartRoom = true;

			Exit localExit = firstRoom.GetComponent<Room>().exits[0];

			currentExits.Add(localExit);

			map.Add(firstRoom);
		}

		while (currentExits.Count() != 0)
		{
			if (map.Count >= maxRooms)
			{
				break;
			}

			if (map.Count >= 10)
			{
				List<GameObject> tempList = new List<GameObject>();

				tempList.AddRange(rooms);
				tempList.AddRange(endRooms);

				availRooms = tempList.ToArray();
			}
			else
			{
				availRooms = rooms;
			}

			Exit tempCurrentExit = currentExits[0];

			int currentExitLength = currentExits.Count();

			List<GameObject> doneRooms = GetRoom(availRooms, currentExits[0].transform.position, currentExits[0].direction, false);

			foreach (Exit exit in currentExits.ToList())
			{
				if (currentExits[0].transform.position == exit.transform.position && exit != currentExits[0])
				{
					currentExits.Remove(exit);
				}
			}

			foreach (GameObject room in map.ToList<GameObject>())
			{
				if (currentExits[0].transform.position == room.transform.position)
				{
					map.Remove(room);
					Destroy(room);
				}
			}

			switch (currentExits[0].direction)
			{
				case "up":

					int rndUp = UnityEngine.Random.Range(0, doneRooms.Count());
					SpawnRoom(doneRooms[rndUp], currentExits[0].gameObject.transform.position, currentExits[0].direction, true);

					break;

				case "down":

					int rndDown = UnityEngine.Random.Range(0, doneRooms.Count());
					SpawnRoom(doneRooms[rndDown], currentExits[0].gameObject.transform.position, currentExits[0].direction, true);

					break;
				case "left":

					int rndLeft = UnityEngine.Random.Range(0, doneRooms.Count());
					SpawnRoom(doneRooms[rndLeft], currentExits[0].gameObject.transform.position, currentExits[0].direction, true);

					break;
				case "right":

					int rndRight = UnityEngine.Random.Range(0, doneRooms.Count());
					SpawnRoom(doneRooms[rndRight], currentExits[0].gameObject.transform.position, currentExits[0].direction, true);

					break;
			}
		}

		foreach (Exit exit in currentExits.ToList())
		{
			if (currentExits[0].transform.position == exit.transform.position && exit != currentExits[0])
			{
				currentExits.Remove(exit);
			}
		}

		foreach (Exit exit in currentExits.ToList())
        {
			List<GameObject> doneRooms = GetRoom(availRooms, exit.transform.position, exit.direction, true);
			int rnd = UnityEngine.Random.Range(0, doneRooms.Count);
			SpawnRoom(doneRooms[rnd], exit.transform.position, exit.direction, false);
        }

		Path.GetComponent<AstarPath>().Scan();

    }

	private void SpawnRoom(GameObject room, Vector2 pos, string direction, bool addExits)
	{
		GameObject tempRoom = Instantiate(room, pos, Quaternion.identity);

		if(pos == new Vector2(0, 0))
        {
			tempRoom.GetComponent<Room>().isStartRoom = true;
        }

		Exit[] localExits = tempRoom.GetComponent<Room>().exits;

        if (addExits)
        {
			foreach (Exit tempExit in localExits)
			{

				switch (direction)
				{
					case "up":
						if (tempExit.direction != "down")
						{
							currentExits.Add(tempExit);
						}
						break;
					case "down":
						if (tempExit.direction != "up")
						{
							currentExits.Add(tempExit);
						}
						break;
					case "left":
						if (tempExit.direction != "right")
						{
							currentExits.Add(tempExit);
						}
						break;
					case "right":
						if (tempExit.direction != "left")
						{
							currentExits.Add(tempExit);
						}
						break;
				}

			}
		}

		map.Add(tempRoom);

		if(addExits != false)
        {
			currentExits.RemoveAt(0);
		}
	}

	private List<GameObject> GetRoom(Array availRooms, Vector3 pos, string direction, bool end)
	{
		bool exitUp = false;
		bool exitDown = false;
		bool exitRight = false;
		bool exitLeft = false;

		bool wallUp = false;
		bool wallDown = false;
		bool wallRight = false;
		bool wallLeft = false;

		foreach (GameObject room in map)
		{
			switch (direction)
			{
				case "up":
					exitDown = true;
					if (room.transform.position == pos + new Vector3(0, 10, 0))
					{
						foreach(Exit exit in room.GetComponent<Room>().exits)
						{
							if(exit.direction == "down")
							{
								exitUp = true;
							}
						}
						if (!exitUp)
						{
							wallUp = true;
						}
					} else if(room.transform.position == pos + new Vector3(10, 0, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "left")
							{
								exitRight = true;
							}
						}
						if (!exitRight)
						{
							wallRight = true;
						}
					} else if(room.transform.position == pos + new Vector3(-10, 0, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "right")
							{
								exitLeft = true;
							}
						}
						if (!exitLeft)
						{
							wallLeft = false;
						}
					}
					break;
				case "down":
					exitUp = true;
					if (room.transform.position == pos + new Vector3(0, -10, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "up")
							{
								exitDown = true;
							}
						}
						if (!exitDown)
						{
							wallDown = true;
						}
					}
					else if (room.transform.position == pos + new Vector3(10, 0, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "left")
							{
								exitRight = true;
							}
						}
						if (!exitRight)
						{
							wallRight = true;
						}
					}
					else if (room.transform.position == pos + new Vector3(-10, 0, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "right")
							{
								exitLeft = true;
							}
						}
						if (!exitLeft)
						{
							wallLeft = true;
						}
					}
					break;
				case "right":
					exitLeft = true;
					if (room.transform.position == pos + new Vector3(0, 10, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "down")
							{
								exitUp = true;
							}
						}
						if (!exitUp)
						{
							wallUp = true;
						}
					}
					else if (room.transform.position == pos + new Vector3(0, -10, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "up")
							{
								exitDown = true;
							}
						}
						if (!exitDown)
						{
							wallDown = true;
						}
					}
					else if (room.transform.position == pos + new Vector3(10, 0, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "left")
							{
								exitRight = true;
							}
						}
						if (!exitRight)
						{
							wallRight = true;
						}
					}
					break;
				case "left":
					exitRight = true;
					if (room.transform.position == pos + new Vector3(0, 10, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "down")
							{
								exitUp = true;
							}
						}
						if (!exitUp)
						{
							wallUp = true;
						}
					}
					else if (room.transform.position == pos + new Vector3(0, -10, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "up")
							{
								exitDown = true;
							}
						}
						if (!exitDown)
						{
							wallDown = true;
						}
					}
					else if (room.transform.position == pos + new Vector3(-10, 0, 0))
					{
						foreach (Exit exit in room.GetComponent<Room>().exits)
						{
							if (exit.direction == "right")
							{
								exitLeft = true;
							}
						}
						if (!exitLeft)
						{
							wallLeft = true;
						}
					}
					break;
			}
		}

		List<GameObject> correctRooms = availRooms.OfType<GameObject>().ToList();

		if (exitUp)
		{
			foreach(GameObject room in correctRooms.ToList<GameObject>())
			{
				bool hasExit = false;
				foreach(Exit exit in room.GetComponent<Room>().exits)
				{
					if(exit.direction == "up")
					{
						hasExit = true;
					}
				}
				if (!hasExit)
				{
					correctRooms.Remove(room);
				}
			}
		}

		if (exitDown)
		{
			foreach (GameObject room in correctRooms.ToList<GameObject>())
			{
				bool hasExit = false;
				foreach (Exit exit in room.GetComponent<Room>().exits)
				{
					if (exit.direction == "down")
					{
						hasExit = true;
					}
				}
				if (!hasExit)
				{
					correctRooms.Remove(room);
				}
			}
		}

		if (exitRight)
		{
			foreach (GameObject room in correctRooms.ToList<GameObject>())
			{
				bool hasExit = false;
				foreach (Exit exit in room.GetComponent<Room>().exits)
				{
					if (exit.direction == "right")
					{
						hasExit = true;
					}
				}
				if (!hasExit)
				{
					correctRooms.Remove(room);
				}
			}
		}

		if (exitLeft)
		{
			foreach (GameObject room in correctRooms.ToList<GameObject>())
			{
				bool hasExit = false;
				foreach (Exit exit in room.GetComponent<Room>().exits)
				{
					if (exit.direction == "left")
					{
						hasExit = true;
					}
				}
				if (!hasExit)
				{
					correctRooms.Remove(room);
				}
			}
		}

		if (wallUp)
		{
			foreach (GameObject room in correctRooms.ToList<GameObject>())
			{
				foreach (Exit exit in room.GetComponent<Room>().exits)
				{
					if(exit.direction == "up")
					{
						correctRooms.Remove(room);
						break;
					}
				}
			}
		}

		if (wallDown)
		{
			foreach (GameObject room in correctRooms.ToList<GameObject>())
			{
				foreach (Exit exit in room.GetComponent<Room>().exits)
				{
					if (exit.direction == "down")
					{
						correctRooms.Remove(room);
						break;
					}
				}
			}
		}

		if (wallRight)
		{
			foreach (GameObject room in correctRooms.ToList<GameObject>())
			{
				foreach (Exit exit in room.GetComponent<Room>().exits)
				{
					if (exit.direction == "right")
					{
						correctRooms.Remove(room);
						break;
					}
				}
			}
		}

		if (wallLeft)
		{
			foreach (GameObject room in correctRooms.ToList<GameObject>())
			{
				foreach (Exit exit in room.GetComponent<Room>().exits)
				{
					if (exit.direction == "left")
					{
						correctRooms.Remove(room);
						break;
					}
				}
			}
		}

		if(end)
        {
            if (!exitUp)
            {
				foreach(GameObject room in correctRooms.ToList<GameObject>())
                {
					foreach(Exit exit in room.GetComponent<Room>().exits)
                    {
						if(exit.direction == "up")
                        {
							correctRooms.Remove(room);
							break;
						}
                    }
                }
            }

			if (!exitDown)
			{
				foreach (GameObject room in correctRooms.ToList<GameObject>())
				{
					foreach (Exit exit in room.GetComponent<Room>().exits)
					{
						if (exit.direction == "down")
						{
							correctRooms.Remove(room);
							break;
						}
					}
				}
			}

			if (!exitRight)
			{
				foreach (GameObject room in correctRooms.ToList<GameObject>())
				{
					foreach (Exit exit in room.GetComponent<Room>().exits)
					{
						if (exit.direction == "right")
						{
							correctRooms.Remove(room);
							break;
						}
					}
				}
			}

			if (!exitLeft)
			{
				foreach (GameObject room in correctRooms.ToList<GameObject>())
				{
					foreach (Exit exit in room.GetComponent<Room>().exits)
					{
						if (exit.direction == "left")
						{
							correctRooms.Remove(room);
							break;
						}
					}
				}
			}
		}

		return correctRooms;
	}
}
