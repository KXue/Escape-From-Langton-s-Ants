using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RepeatingRule{
	public int repeatCount = 1;
	public MapRule m_rule = new CaveRule();
	public bool[][] ApplyRuleRepeatedly(bool [][] map){
		bool [][] returnMap = new bool[0][];
		for(int i = 0; i < repeatCount; i++){
			returnMap = m_rule.ApplyRule(map);
			map = returnMap;
		}
		return returnMap;
	}
}
public class Helper{
	// right, up, left, down (screen coordinates)
	public static int[][] CARDINALDIRECTIONS = {new int[]{1, 0}, new int[]{0, -1}, new int[]{-1, 0}, new int[]{0, 1}};
	public static bool MapValue(bool[][] map, int row, int col, bool wrap){
		if(wrap){
			row = BetterMod(row, map.Length);
			col = BetterMod(col, map[row].Length);
			return map[row][col];
		}
		else if(row >= 0  && row < map.Length && col >= 0 && col < map[row].Length){
			return map[row][col];
		}
		else{
			return true;
		}
	}
	public static void SetMapValue(bool[][] map, int row, int col, bool wrap, bool value){
		if(wrap){
			row = BetterMod(row, map.Length);
			col = BetterMod(col, map[row].Length);
			map[row][col] = value;
		}
		else if(row >= 0  && row < map.Length && col >= 0 && col < map[row].Length){
			map[row][col] = value;
		}
	}
	static int BetterMod(int x, int m) {
		int r = x%m;
		return r<0 ? r+m : r;
	}
	public static int MooreNeighborWallCount(bool[][] map, int row, int col, int radius, bool wrap){
		int count = 0;
		for(int rowOffset = -radius; rowOffset <= radius; rowOffset++){
			for(int colOffset = -radius; colOffset <= radius; colOffset++){
				if((rowOffset != 0 || colOffset != 0) && MapValue(map, row + rowOffset, col + colOffset, wrap)){
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
		if(Helper.MapValue(map, row, col, wrap) || Helper.MapValue(travelled, row, col, wrap)){
			return;
		}else{
			Helper.SetMapValue(travelled, row, col, wrap, true);
			count++;
			for(int i = 0; i < CARDINALDIRECTIONS.Length; i++){
				FloodFillMap(map, row + CARDINALDIRECTIONS[i][0], col + CARDINALDIRECTIONS[i][1], travelled, ref count, wrap);
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
}
public abstract class MapRule{
	public bool m_wrap;
	public bool[][] ApplyRule(bool[][] prevIteration){
		bool[][] nextIteration = new bool[prevIteration.Length][];

		for(int row = 0; row < prevIteration.Length; row++){
			nextIteration[row] = new bool[prevIteration[row].Length];
			for(int col = 0; col < prevIteration[row].Length; col++){
				nextIteration[row][col] = TestCell(prevIteration, row, col);
			}
		}
		return nextIteration;
	}
	protected abstract bool TestCell(bool[][] map, int row, int col);
}
public class CaveRule : MapRule
{
	protected override bool TestCell(bool[][] map, int row, int col){
		int immediateNeighbours = Helper.MooreNeighborWallCount(map, row, col, 1, m_wrap);
		int extendedNeighbours = Helper.MooreNeighborWallCount(map, row, col, 2, m_wrap);
		return immediateNeighbours >= 5 || extendedNeighbours <= 2;
	}
}

public class SmoothRule: MapRule{
	protected override bool TestCell(bool[][] map, int row, int col){
		int immediateNeighbours = Helper.MooreNeighborWallCount(map, row, col, 1, m_wrap);
		return immediateNeighbours >= 5;
	}
}
