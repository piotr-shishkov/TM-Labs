using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CellSpawner : MonoBehaviour
{

	public Cell cellPrefab;
	public GameObject cellsHolder;

	public CellEngine engine;

	public int cellPerStep = 100;

	[HideInInspector] public Cell[,] cells;
	[HideInInspector] public int size;

	[HideInInspector] public Action<int> spawnProgress;
	[HideInInspector] public Action spawnFinish;

	public int[] sizes = new int[] { };

	private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

	void Start()
	{
		spawnFinish += FinishSpawning;
	}

	public void StartSpawning(int i)
	{
		stopwatch.Reset();
		stopwatch.Start();
		size = sizes[i];
		StartCoroutine(SpawnCoroutine(size));
	}

	private void FinishSpawning()
	{
		stopwatch.Stop();
	}

	IEnumerator SpawnCoroutine(int x)
	{
		cells = new Cell[x, x];
		engine.cells = cells;
		int totalCells = x * x;
		int nextBreak = cellPerStep;
		int lastX = 0;
		int spawnedCells = 0;

		if (spawnProgress != null)
			spawnProgress(0);
		while (spawnedCells != totalCells)
		{
			for (int i = lastX; i < nextBreak; i++)
			{
				CreateCell(i % x, (i / x));
				spawnedCells++;
				if (spawnProgress != null)
					spawnProgress(Mathf.RoundToInt(spawnedCells / (float)totalCells * 100f));
			}
			lastX += cellPerStep;
			nextBreak += cellPerStep;
			if (nextBreak > totalCells)
				nextBreak = totalCells;
			yield return null;
		}

		SetNeighbours(x);

		if (spawnProgress != null)
			spawnProgress(100);
		if (spawnFinish != null)
			spawnFinish();
	}

	private void CreateCell(int i, int j)
	{
		Cell c = Instantiate(cellPrefab, new Vector3((float)i + (size / 4), (float)j, 0f), Quaternion.identity) as Cell;
		cells[i, j] = c;
		c.Init(cellsHolder.transform, i, j);
		c.SetRandomState();
		engine.cellUpdates += c.CellUpdate;
		engine.cellApplyUpdates += c.CellApplyUpdate;
	}

	private void SetNeighbours(int x)
	{
		for (int i = 0; i < x; i++)
			for (int j = 0; j < x; j++)
				cells[i, j].neighbours = GetNeighbours(i, j);
	}

	private Cell[] GetNeighbours(int x, int y)
	{
		List<Cell> myCells = new List<Cell>(); 
		if (y + 1 < size)
			myCells.Add(cells[x, (y + 1)]); // top

		if (x + 1 < size && y + 1 < size)
			myCells.Add(cells[(x + 1), (y + 1)]); // top right

		if (x + 1 < size)
			myCells.Add(cells[(x + 1), y]); // right

		if (x + 1 < size && y - 1 < size && y > 0)
			myCells.Add(cells[(x + 1), (y - 1)]); // bottom right

		if (y - 1 < size && y > 0)
			myCells.Add(cells[x, (y - 1)]); // bottom

		if (x - 1 < size && x>0 && y - 1 < size && y > 0)
			myCells.Add(cells[( x - 1), (y - 1)]); // bottom left

		if (x - 1 < size && x>0)
			myCells.Add(cells[(x - 1), y]); // left

		if (x - 1 < size && y + 1 < size && x > 0)
			myCells.Add(cells[(size - x - 1), (y + 1) ]); // top left

		return myCells.ToArray();
	}
}
