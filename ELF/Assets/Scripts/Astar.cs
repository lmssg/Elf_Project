//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SocialPlatforms;


//namespace TinyJoy.SSG.Game.Map.PathFinding
//{
//    public class AStarPoint
//    {
//        public static AStarPoint create(int x, int y)
//        {
//            return new AStarPoint(x, y);
//        }

//        public void Reset()
//        {
//            Info[0] = 0;
//            Info[1] = 0;
//            Info[2] = 0;
//            Info[3] = 0;
//            Info[4] = 0;
//            parent = null;
//        }

//        private AStarPoint(int _x, int _y)
//        {
//            Info[0] = (short)_x;
//            Info[1] = (short)_y;
//        }

//        //public int x, y; //点坐标，这里为了方便按照C++的数组来计算，x代表横排，y代表竖列
//        //public int F, G, H; //F=G+H

//        public short[] Info = new short[6]; // x 0, y 1, F 2,G 3, H 4, usedCount 5
//        public AStarPoint parent; //parent的坐标，这里没有用指针，从而简化代码
//        //public int usedCount;
//    };

//    public class PointList
//    {
//        public int Count { get => points.Count; }
//        private List<AStarPoint> points;

//        public AStarPoint this[int index] { get => points[index]; }
//        public PointList(int count)
//        {
//            points = new List<AStarPoint>();
//        }

//        public void Clear()
//        {
//            for (int i = 0; i < Count; i++)
//            {
//                AStarPoint point = points[i];
//                if (point != null)
//                {
//                    point.Info[5]--;
//                }
//            }

//            //Array.Clear(points, 0, Count);
//            points.Clear();
//        }

//        public void AddPoint(AStarPoint point)
//        {
//            point.Info[5]++;

//            points.Add(point);
//        }

//        public void RemovePoint(AStarPoint point)
//        {
//            point.Info[5]--;

//            points.Remove(point);
//        }

//        public void AddRangePoint(PointList other)
//        {
//            for (int i = 0; i < other.Count; i++)
//            {
//                AddPoint(other.points[i]);
//            }
//        }
//    }

//    public class Astar
//    {
//        const int kCost1 = 10; //直移一格消耗
//        const int kCost2 = 12; //斜移一格消耗

//        private static int[] vMaze;

//        private static int nWidth;
//        private static int nHeight;

//        private PointList vOpenList;     //开启列表
//        private PointList vCloseList;        //关闭列表

//        private List<AStarPoint> vPointCache;		// Pool

//        private PointList vSurroundPoints;

//        private static BoundsInt mapBounds;

//        private int cacheIndex = 0;

//        private PointList vTempList = new PointList(1000);

//       // private MapService mapService;

//        PointList tempPathPoints = new PointList(1000);
//        //public Astar(MapService mapService)
//        //{
//        //    this.mapService = mapService;

//        //    vSurroundPoints = new PointList(8);
//        //    vOpenList = new PointList(1000);
//        //    vCloseList = new PointList(1000);
//        //    vPointCache = new List<AStarPoint>(2000);
//        //}

//        public static bool InitAstar(int[] maze, BoundsInt bounds)
//        {
//            if (maze.Length == 0
//                || bounds == null
//            || bounds.size.x <= 0
//            || bounds.size.y <= 0
//            )
//            {
//                return false;
//            }

//            vMaze = maze;
//            nWidth = bounds.size.x;
//            nHeight = bounds.size.y;
//            mapBounds = bounds;

//            return true;
//        }

//        public void RestOpenAndCloseList()
//        {
//            vCloseList.Clear();
//            vOpenList.Clear();
//        }

//        public bool GetPathByCell(PointList pathPoints, Vector3Int startPoint, Vector3Int endPoint, bool isIgnoreCorner = false)
//        {
//            if (vMaze == null)
//            {
//                return false;
//            }

//            if (pathPoints == null)
//            {
//                return false;
//            }

//            // 清空临时开闭列表，防止重复执行GetPath导致结果异常
//            RestOpenAndCloseList();
//            //pathPoints.Clear();

//            startPoint -= mapBounds.min;
//            endPoint -= mapBounds.min;

//            AStarPoint result = FindPath(startPoint, endPoint, isIgnoreCorner);

//            if (result == null)
//            {
//                return false;
//            }

//            //返回路径，如果没找到路径
//            while (result != null)
//            {
//                vTempList.AddPoint(result);
//                result = result.parent;
//            }

//            for (int i = vTempList.Count - 1; i >= 0; i--)
//            {
//                pathPoints.AddPoint(vTempList[i]);
//            }

//            vTempList.Clear();

//            return true;
//        }

//        public void CheckIsValuable(PointList pathPoints)
//        {
//            if (pathPoints.Count == 0)
//            {
//                return;
//            }

//            PointList tempPointList = new PointList(1000);
//            tempPointList.AddRangePoint(pathPoints);
//            pathPoints.Clear();

//            int lastX = 0;
//            int lastY = 0;
//            AStarPoint lastPoint = null;
//            int nSize = tempPointList.Count;

//            pathPoints.AddPoint(tempPointList[0]);

//            for (int i = 1; i < nSize; i++)
//            {
//                var point = tempPointList[i];

//                if (lastPoint == null)
//                {
//                    lastPoint = point;
//                    continue;
//                }

//                if (point.Info[0] == lastPoint.Info[0] && point.Info[1] == lastPoint.Info[1])
//                {
//                    continue;
//                }

//                if (point.Info[0] - lastPoint.Info[0] == lastX
//                    && point.Info[1] - lastPoint.Info[1] == lastY
//                )
//                {
//                    lastPoint = point;
//                }
//                else
//                {
//                    pathPoints.AddPoint(lastPoint);
//                    lastX = point.Info[0] - lastPoint.Info[0];
//                    lastY = point.Info[1] - lastPoint.Info[1];
//                    lastPoint = point;
//                }

//                if (i >= nSize - 1)
//                {
//                    pathPoints.AddPoint(point);
//                }
//            }

//            tempPointList.Clear();
//        }

//        public void Clear() { }


//        public AStarPoint FindPath(Vector3Int startPoint, Vector3Int endPoint, bool isIgnoreCorner)
//        {
//            vOpenList.AddPoint(CreatePoint(startPoint.x, startPoint.y)); //置入起点,拷贝开辟一个节点，内外隔离
//            AStarPoint pEndPoint = CreatePoint(endPoint.x, endPoint.y);
//            pEndPoint.Info[5]++;

//            while (vOpenList.Count > 0)
//            {
//                var curPoint = getLeastFpoint(); //找到F值最小的点

//                vCloseList.AddPoint(curPoint); //放到关闭列表
//                vOpenList.RemovePoint(curPoint); //从开启列表中删除

//                //1,找到当前周围八个格中可以通过的格子
//                getSurroundPoints(curPoint, isIgnoreCorner);

//                for (int i = 0; i < vSurroundPoints.Count; i++)
//                {
//                    var target = vSurroundPoints[i];
//                    //2,对某一个格子，如果它不在开启列表中，加入到开启列表，设置当前格为其父节点，计算F G H
//                    if (isInList(vOpenList, target) == null)
//                    {
//                        target.parent = curPoint;

//                        target.Info[3] = calcG(curPoint, target);
//                        target.Info[4] = calcH(target, pEndPoint);
//                        target.Info[2] = calcF(target);

//                        vOpenList.AddPoint(target);
//                    }
//                    //3，对某一个格子，它在开启列表中，计算G值, 如果比原来的大, 就什么都不做, 否则设置它的父节点为当前点,并更新G和F
//                    else
//                    {
//                        short tempG = calcG(curPoint, target);
//                        if (tempG < target.Info[3])
//                        {
//                            target.parent = curPoint;

//                            target.Info[3] = tempG;
//                            target.Info[2] = calcF(target);
//                        }
//                    }

//                    //AStarPoint resPoint = isInList(vOpenList, pEndPoint);
//                    if (pEndPoint.Info[0] == target.Info[0] && pEndPoint.Info[1] == target.Info[1])
//                    {
//                        pEndPoint.Info[5]--;

//                        return target; //返回列表里的节点指针
//                    }
//                }
//            }

//            pEndPoint.Info[5]--;
//            return null;
//        }

//        private void getSurroundPoints(AStarPoint point, bool isIgnoreCorner)
//        {
//            vSurroundPoints.Clear();
//            for (int x = point.Info[0] - 1; x <= point.Info[0] + 1; x++)
//            {
//                for (int y = point.Info[1] - 1; y <= point.Info[1] + 1; y++)
//                {
//                    AStarPoint newPoint = CreatePoint(x, y);
//                    if (isCanreach(point, newPoint, isIgnoreCorner))
//                    {
//                        vSurroundPoints.AddPoint(newPoint);
//                    }
//                }
//            }
//        }

//        bool isCanreach(AStarPoint point, AStarPoint target, bool isIgnoreCorner) //判断某点是否可以用于下一步判断
//        {
//            int nTargetIndex = target.Info[0] + target.Info[1] * nWidth;
//            int nSize = vMaze.Length;

//            if (nTargetIndex < 0
//                || nTargetIndex >= nSize
//                || (vMaze[nTargetIndex] & (int)EnPhyFlag.CANT_MOVE) != 0
//                || (target.Info[0] == point.Info[0] && target.Info[1] == point.Info[1])
//                || isInList(vCloseList, target) != null) //如果点与当前节点重合、超出地图、是障碍物、或者在关闭列表中，返回false
//            {
//                return false;
//            }
//            else
//            {
//                if (Math.Abs(point.Info[0] - target.Info[0]) + Math.Abs(point.Info[1] - target.Info[1]) == 1) //非斜角可以
//                {
//                    return true;
//                }
//                else
//                {
//                    int nPxTyIndex = point.Info[0] + target.Info[1] * nWidth;
//                    int nTxPyIndex = target.Info[0] + point.Info[1] * nWidth;

//                    if (nPxTyIndex < 0
//                        || nPxTyIndex >= nSize
//                        || nTxPyIndex < 0
//                        || nTxPyIndex >= nSize
//                        )
//                    {
//                        return false;
//                    }

//                    //斜对角要判断是否绊住
//                    if ((vMaze[nPxTyIndex] & (int)EnPhyFlag.CANT_MOVE) == 0
//                    && (vMaze[nTxPyIndex] & (int)EnPhyFlag.CANT_MOVE) == 0
//                    )
//                    {
//                        return true;
//                    }
//                    else
//                    {
//                        return isIgnoreCorner;
//                    }
//                }
//            }
//        }
//        AStarPoint isInList(PointList list, AStarPoint point)  //判断开启/关闭列表中是否包含某点
//        {
//            //判断某个节点是否在列表中，这里不能比较指针，因为每次加入列表是新开辟的节点，只能比较坐标
//            for (int i = list.Count - 1; i >= 0; i--)
//            {
//                if (list[i].Info[0] == point.Info[0]
//                && list[i].Info[1] == point.Info[1]
//                )
//                    return list[i];
//            }

//            return null;
//        }
//        AStarPoint getLeastFpoint() //从开启列表中返回F值最小的节点
//        {
//            if (vOpenList.Count > 0)
//            {
//                var resPoint = vOpenList[0];
//                for (int i = 1; i < vOpenList.Count; i++)
//                {
//                    var point = vOpenList[i];
//                    if (point.Info[2] < resPoint.Info[2])
//                        resPoint = point;
//                }

//                return resPoint;
//            }

//            return null;
//        }
//        //计算FGH值
//        short calcG(AStarPoint temp_start, AStarPoint point)
//        {
//            int extraG = (Math.Abs(point.Info[0] - temp_start.Info[0]) + Math.Abs(point.Info[1] - temp_start.Info[1])) == 1 ? kCost1 : kCost2;
//            int parentG = point.parent == null ? 0 : point.parent.Info[3]; //如果是初始节点，则其父节点是空

//            return (short)(parentG + extraG);
//        }
//        short calcH(AStarPoint point, AStarPoint end)
//        {
//            //估算法
//            int x = Mathf.Abs(point.Info[0] - end.Info[0]);
//            int y = Mathf.Abs(point.Info[1] - end.Info[1]);
//            //根号2 = 1.4  然后都扩大10倍 去除小数计算,这里返回值都放大了10倍
//            if (x > y)
//            {
//                return (short)(14 * y + 10 * (x - y));
//            }
//            else
//            {
//                return (short)(14 * x + 10 * (y - x));
//            }
//        }
//        short calcF(AStarPoint point)
//        {
//            return (short)(point.Info[3] + point.Info[4]);
//        }

//        public AStarPoint CreatePoint(int x, int y)
//        {
//            var cacheCount = vPointCache.Count;
//            for (int i = 0; i < cacheCount; i++)
//            {
//                if (cacheIndex >= cacheCount)
//                {
//                    cacheIndex = 0;
//                }

//                var point = vPointCache[cacheIndex++];
//                if (point.Info[5] == 0)
//                {
//                    point.Reset();

//                    point.Info[0] = (short)x;
//                    point.Info[1] = (short)y;

//                    return point;
//                }
//            }

//            var retPoint = AStarPoint.create(x, y);
//            vPointCache.Add(retPoint);

//            //DebugLogger.Debug("cacheCount = " + cacheCount);
//            return retPoint;
//        }

//        public List<Vector3Int> GetPath(Vector3Int startPos, Vector3Int endPos, bool isIgnoreCorner = false)
//        {
//            List<Vector3Int> pathPoints = new List<Vector3Int>();

//            if (startPos == endPos)
//            {
//                return pathPoints;
//            }

//            if (mapService.CellPosCanMove(startPos) == false || mapService.CellPosCanMove(endPos) == false)
//            {
//                return pathPoints;
//            }

//            tempPathPoints.Clear();

//            if (mapService.CellPosIsInPutArea(startPos) && mapService.CellPosIsInPutArea(endPos) == false)
//            {
//                if (GetPathByCell(tempPathPoints, startPos, mapService.DoorPathPosList[1], isIgnoreCorner) == false)
//                {
//                    return pathPoints;
//                }

//                if (endPos.Equals(mapService.LeavePos))
//                {
//                    tempPathPoints.AddRangePoint(mapService.CachePathPoints[enCachPathPoint.DoorToLeave]);
//                }
//                else if (endPos.Equals(mapService.SpeLeavePos))
//                {
//                    tempPathPoints.AddRangePoint(mapService.CachePathPoints[enCachPathPoint.DoorToSpeLeave]);
//                }
//                else
//                {
//                    if (endPos.Equals(mapService.DoorPathPosList[1]) == false && GetPathByCell(tempPathPoints, mapService.DoorPathPosList[1], endPos, isIgnoreCorner) == false)
//                    {
//                        return pathPoints;
//                    }
//                }
//            }
//            else if (startPos.Equals(mapService.BornPos) && endPos.Equals(mapService.DoorPathPosList[2]))
//            {
//                tempPathPoints.AddRangePoint(mapService.CachePathPoints[enCachPathPoint.BornToDoor]);
//            }
//            else if (startPos.Equals(mapService.SpeBornPos) && endPos.Equals(mapService.DoorPathPosList[2]))
//            {
//                tempPathPoints.AddRangePoint(mapService.CachePathPoints[enCachPathPoint.SpeBornToDoor]);
//            }
//            else
//            {
//                if (GetPathByCell(tempPathPoints, startPos, endPos, isIgnoreCorner) == false)
//                {
//                    return pathPoints;
//                }
//            }

//            CheckIsValuable(tempPathPoints);

//            for (int i = 0; i < tempPathPoints.Count; i++)
//            {
//                pathPoints.Add(new Vector3Int(tempPathPoints[i].Info[0] + mapBounds.xMin, tempPathPoints[i].Info[1] + mapBounds.yMin, 0));
//            }

//            return pathPoints;
//        }
//    };


//    public enum EnPhyFlag
//    {
//        CANT_MOVE = 1, //不可行走
//        CANT_USE = 2, //不可用
//    }
//}