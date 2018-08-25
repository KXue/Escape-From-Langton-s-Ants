using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardinalDirections {RIGHT, UP, LEFT, DOWN, END}
public class Helper{
	// right, up, left, down (screen coordinates)
	public static int[][] DIRECTIONS = {new int[]{1, 0}, new int[]{0, -1}, new int[]{-1, 0}, new int[]{0, 1}};
	public static bool MapValue(bool[][] map, ref int row, ref int col, bool wrap){
		if(wrap){
			GetWrappedCoordinates(map, ref row, ref col);
			return map[row][col];
		}
		else if(row >= 0  && row < map.Length && col >= 0 && col < map[row].Length){
			return map[row][col];
		}
		else{
			return true;
		}
	}
	public static void SetMapValue(bool[][] map, ref int row, ref int col, bool wrap, bool value){
		if(wrap){
			GetWrappedCoordinates(map, ref row, ref col);
			map[row][col] = value;
		}
		else if(row >= 0  && row < map.Length && col >= 0 && col < map[row].Length){
			map[row][col] = value;
		}
	}
	public static void GetWrappedCoordinates(bool [][] map, ref int row, ref int col){
		row = BetterMod(row, map.Length);
		col = BetterMod(col, map[row].Length);
	}
	static int BetterMod(int x, int m) {
		int r = x%m;
		return r<0 ? r+m : r;
	}
	public static int MooreNeighborWallCount(bool[][] map, int row, int col, int radius, bool wrap){
		int count = 0;
		for(int rowOffset = -radius; rowOffset <= radius; rowOffset++){
			for(int colOffset = -radius; colOffset <= radius; colOffset++){
				int newRow = row + rowOffset;
				int newCol = col + colOffset;
				if((rowOffset != 0 || colOffset != 0) && MapValue(map, ref newRow, ref newCol, wrap)){
					count++;
				}
			}
		}
		return count;
	}
	public static void PrintMap(bool [][] map){
		string printString = "\n";
		for(int row = 0; row < map.Length; row++){
			for(int col = 0; col < map[0].Length; col++){
				printString += map[row][col] ? '1' : '0';
			}
			printString += '\n';
		}
		Debug.Log(printString);
	}
	public static bool IsMapValid(bool [][] map, bool wrap){
		int totalMapArea = TotalMapArea(map);
		
		bool[][] travelled = new bool[map.Length][];
		for(int row = 0; row < map.Length; row++){
			travelled[row] = new bool[map[row].Length];
			for(int col = 0; col < map[row].Length; col++){
				travelled[row][col] = false;
			}
		}
		int count = 0;
		int startRow, startCol;
		FirstEmptyTile(map, out startRow, out startCol);

		FloodFillMap(map, startRow, startCol, travelled, ref count, wrap);

		return totalMapArea == count;
	}
	public static int TotalMapArea(bool [][] map){
		int area = 0;
		for(int row = 0; row < map.Length; row++){
			for(int col = 0; col < map[row].Length; col++){
				if(!map[row][col]){
					area++;
				}
			}
		}
		return area;
	}
	public static void FloodFillMap(bool [][] map, int row, int col, bool[][] travelled, ref int count, bool wrap){
		if(Helper.MapValue(map, ref row, ref col, wrap) || Helper.MapValue(travelled, ref row, ref col, wrap)){
			return;
		}else{
			Helper.SetMapValue(travelled, ref row, ref col, wrap, true);
			count++;
			for(int i = 0; i < DIRECTIONS.Length; i++){
				FloodFillMap(map, row + DIRECTIONS[i][0], col + DIRECTIONS[i][1], travelled, ref count, wrap);
			}
		}
	}
	public static void FirstEmptyTile(bool [][] map, out int row, out int col){
		row = -1;
		col = -1;
		for(int i = 0; i < map.Length; i++){
			for(int j = 0; j < map[i].Length; j++){
				if(!map[i][j]){
					row = i;
					col = j;
					return;
				}
			}
		}
	}
    public static void FindFurthestPoints(Vector3[] points, out Vector3 firstPoint, out Vector3 secondPoint)
    {
		firstPoint = points[0];
		secondPoint = points[1];
		float biggestDistance = 0f;
		for(int i = 0; i < points.Length - 1; i++){
			for(int j = i + 1; j < points.Length; j++){
				float distance = Vector3.Distance(points[i], points[j]);
				if(distance > biggestDistance){
					biggestDistance = distance;
					firstPoint = points[i];
					secondPoint = points[j];
				}
			}
		}
    }
}
