using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_22 : DayScript2021
{
    protected override string part_1()
    {
        List<Cuboid> cubes = new List<Cuboid>();
        foreach (string instruction in _input.Split('\n'))
        {
            Debug.LogError("instruction is " + instruction);

            bool isOn = instruction.Split(' ')[0] == "on";
            List<(int, int)> bounds = new List<(int, int)>();
            foreach (string coords in instruction.Split(' ')[1].Split(','))
            {
                int min = Mathf.Max(-50, int.Parse(coords.Substring(2, coords.IndexOf('.') - 2)));
                int max = Mathf.Min(50, int.Parse(coords.Substring(coords.LastIndexOf('.') + 1)));

                if (max < -50 || min > 50)
                    continue;

                bounds.Add((min, max));
            }

            if (bounds.Count != 3)
            {
                Debug.Log("SKIPPING INSTRUCTION : " + instruction);
                continue;
            }

            Cuboid currentCube = new Cuboid(new Point3D(bounds[0].Item1, bounds[1].Item1, bounds[2].Item1), new Point3D(bounds[0].Item2, bounds[1].Item2, bounds[2].Item2));
            //Debug.Log(currentCube.ToString() + " has volume " + currentCube.GetVolume().ToString());
            //if (cubes.Count > 0)
            //{
            //    Debug.Log("** has intersection with cube " + cubes[0].ToString() + " that is : " + currentCube.GetIntersectionVolume(cubes[0]).ToString());

            //    string log = " * * Subcubes without intersection are : " + System.Environment.NewLine;
            //    int subVolume = 0;
            //    foreach (Cuboid subcube in currentCube.GetSubstractedCubes(cubes[0]))
            //    {
            //        log += subcube.ToString() + System.Environment.NewLine;
            //        subVolume += subcube.GetVolume();
            //    }
            //    Debug.Log(log);
            //    Debug.Log(" * *  and their volume is " + subVolume);
            //    //Debug.Log("--> " + cube.GetRemainingVolumeWithoutIntersections(cubes));
            //}

            if (isOn)
            {
                List<Cuboid> newCubes = currentCube.GetRemainingCubesWithoutIntersections(new List<Cuboid>(cubes));
                string log = " new cubes of addition are : " + System.Environment.NewLine;
                foreach (Cuboid subcube in newCubes)
                {
                    log += subcube.ToString() + System.Environment.NewLine;
                }
                cubes.AddRange(newCubes);
                Debug.Log(log);
                Debug.Log(" so now cubes count is " + cubes.Count + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, cubes.ConvertAll<string>(x => x.ToString())));

                for (int i = 0; i < cubes.Count; i++)
                {
                    Cuboid current = cubes[i];
                    for (int cnt = 0; cnt < i; cnt++)
                    {
                        if (cubes[cnt].HasIntersection(current))
                        {
                            Debug.LogError("cube index " + cnt + " : " + cubes[cnt].ToString() + " has intersection with index " + i + " : " + current.ToString());
                            return "pouet";
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Before removing, cubes count = " + cubes.Count + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, cubes.ConvertAll<string>(x => x.ToString())));
                Debug.Log("Removing " + currentCube.ToString());

                List<Cuboid> newCubes = new List<Cuboid>();
                foreach (Cuboid tmpCube in cubes)
                {
                    newCubes.AddRange(tmpCube.GetSubstractedCubes(currentCube, false));
                }
                cubes = new List<Cuboid>(newCubes);
                Debug.Log("After removing, cubes count = " + cubes.Count + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, cubes.ConvertAll<string>(x => x.ToString())));

            }
        }        

        long volume = 0;
        foreach (Cuboid cub in cubes)
        {
            
            volume += cub.GetVolume();
        }

        return volume.ToString();
    }

    protected override string part_2()
    {
        long count = 0;
        List<Cuboid> cubes = new List<Cuboid>();
        foreach (string instruction in _input.Split('\n'))
        {
            count++;

            Debug.LogWarning("instruction " + count + " is " + instruction);

            bool isOn = instruction.Split(' ')[0] == "on";

            List<(int, int)> bounds = new List<(int, int)>();
            foreach (string coords in instruction.Split(' ')[1].Split(','))
            {
                int min = int.Parse(coords.Substring(2, coords.IndexOf('.') - 2));
                int max = int.Parse(coords.Substring(coords.LastIndexOf('.') + 1));

                bounds.Add((min, max));
            }

            Cuboid currentCube = new Cuboid(new Point3D(bounds[0].Item1, bounds[1].Item1, bounds[2].Item1), new Point3D(bounds[0].Item2, bounds[1].Item2, bounds[2].Item2));
            Debug.Log(currentCube.ToString());

            bool debug = false; // (count > 11 && count < 17) || count == 38;

            if (isOn)
            {
                List<Cuboid> newCubes = currentCube.GetRemainingCubesWithoutIntersections(new List<Cuboid>(cubes), debug);
                cubes.AddRange(newCubes);
                
                //if (debug)
                //{
                //    string log = " new cubes of addition are : " + System.Environment.NewLine;
                //    foreach (Cuboid subcube in newCubes)
                //    {
                //        log += subcube.ToString() + System.Environment.NewLine;
                //    }
                //    Debug.Log(log);
                //    Debug.Log(" so now cubes count is " + cubes.Count + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, cubes.ConvertAll<string>(x => x.ToString())));
                //}

                //for (int i = 0; i < cubes.Count; i++)
                //{
                //    Cuboid current = cubes[i];
                //    for (int cnt = 0; cnt < i; cnt++)
                //    {
                //        if (cubes[cnt].HasIntersection(current))
                //        {
                //            Debug.LogError("cube index " + cnt + " : " + cubes[cnt].ToString() + " has intersection with index " + i + " : " + current.ToString());
                //            return "pouet";
                //        }
                //    }
                //}
            }
            else
            {
                //if (debug)
                //{
                //    Debug.Log("Before removing, cubes count = " + cubes.Count + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, cubes.ConvertAll<string>(x => x.ToString())));
                //    Debug.Log("Removing " + currentCube.ToString());
                //}

                List<Cuboid> newCubes = new List<Cuboid>();
                foreach (Cuboid tmpCube in cubes)
                {
                    newCubes.AddRange(tmpCube.GetSubstractedCubes(currentCube, debug));
                }
                cubes = new List<Cuboid>(newCubes);
                
                //if (debug)
                //    Debug.Log("After removing, cubes count = " + cubes.Count + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, cubes.ConvertAll<string>(x => x.ToString())));

            }
        }

        long volume = 0;
        foreach (Cuboid cub in cubes)
        {
            volume += cub.GetVolume();
        }

        return volume.ToString();
    }

    class Cuboid
    {
        public Point3D minPoint;
        public Point3D maxPoint;

        public Cuboid(Point3D min, Point3D max)
        {
            this.minPoint = min;
            this.maxPoint = max;
        }

        public long GetVolume()
        {
            return (1+ ((long)maxPoint.x - (long)minPoint.x)) * (1+ ((long)maxPoint.y - (long)minPoint.y)) * (1+ ((long)maxPoint.z - (long)minPoint.z));
        }

        public bool HasIntersection (Cuboid other)
        {
            return !(other.minPoint.x > this.maxPoint.x || other.maxPoint.x < this.minPoint.x
                || other.minPoint.y > this.maxPoint.y || other.maxPoint.y < this.minPoint.y
                || other.minPoint.z > this.maxPoint.z || other.maxPoint.z < this.minPoint.z);
        }

        public bool IsInside(Cuboid other)
        {
            return (other.maxPoint.x >= this.maxPoint.x && other.minPoint.x <= this.minPoint.x
                && other.maxPoint.y >= this.maxPoint.y && other.minPoint.y <= this.minPoint.y
                && other.maxPoint.z >= this.maxPoint.z && other.minPoint.z <= this.minPoint.z);
        }
        public int GetIntersectionVolume(Cuboid other)
        {
            if (!HasIntersection(other))
            {
                Debug.Log("No intersection between cuboid " + this.ToString() + " and " + other.ToString());
                return 0;
            }

            int xRange = Mathf.Min(other.maxPoint.x, this.maxPoint.x) - Mathf.Max(other.minPoint.x, this.minPoint.x);
            int yRange = Mathf.Min(other.maxPoint.y, this.maxPoint.y) - Mathf.Max(other.minPoint.y, this.minPoint.y);
            int zRange = Mathf.Min(other.maxPoint.z, this.maxPoint.z) - Mathf.Max(other.minPoint.z, this.minPoint.z);

            if (xRange <= 0 || yRange <= 0 || zRange <= 0)
            {
                Debug.LogError("Range are incorrect for cuboid " + this.ToString() + " and " + other.ToString());
                return 0;
            }

            return (xRange+1) * (yRange+1) * (zRange+1);

        }

        public List<Cuboid> GetRemainingCubesWithoutIntersections(List<Cuboid> otherCubes, bool debug = false, int offset = 0)
        {
            if (debug)
                Debug.Log(Tools.writeOffset(offset) + " Getting remaining cubes of " + this.ToString() + " by [ " + System.String.Join(" | ", otherCubes.ConvertAll<string>(x => x.ToString())) + " ] ");

            if (otherCubes.Count == 0)
            {
                if (debug)
                    Debug.Log(Tools.writeOffset(offset) + " List of others is empty so return this cube");

                return new List<Cuboid>() { this };
            }

            //Cuboid otherHead = otherCubes[0];
            //List<Cuboid> otherTail = new List<Cuboid>(otherCubes);
            //otherTail.RemoveAt(0);
            //subCubes = GetSubstractedCubes(otherHead, debug, offset);

            //List<Cuboid> result = new List<Cuboid>();
            //foreach (Cuboid cube in subCubes)
            //{
            //    result.AddRange(cube.GetRemainingCubesWithoutIntersections(otherTail, debug, offset+1));
            //}

            List<Cuboid> result = new List<Cuboid>() { this };
            for (int i = 0; i < otherCubes.Count; i++)
            {
                Cuboid otherHead = otherCubes[i];

                List<Cuboid> tmp = new List<Cuboid>();
                foreach (Cuboid sub in result)
                {
                    tmp.AddRange(sub.GetSubstractedCubes(otherHead, debug, offset));
                }

                result = new List<Cuboid>(tmp);
            }

            return result;
        }

        public List<Cuboid> GetSubstractedCubes(Cuboid other, bool debug = false, int offset = 0)
        {
            if (this.IsInside(other))   // if this cuboid is inside the other, we have no substracted cube
            {
                if (debug)
                    Debug.Log(Tools.writeOffset(offset) + this.ToString() + " is inside current removal cube " + other.ToString() + ". Therefore, there is no cube left, returns empty list");

                return new List<Cuboid>();  // return empty
            }

            if (!this.HasIntersection(other))   // if not intersection, simply return cuboid himself as you don't have to substract common parts
            {
                if (debug)
                    Debug.Log(Tools.writeOffset(offset) + this.ToString() + " has no intersection with current removal cube " + other.ToString() + ". Therefore, return full cube (list with 1 item)");

                return new List<Cuboid>() { this };
            }

            List<Cuboid> result = new List<Cuboid>();
            { 
                if (this.minPoint.x < other.minPoint.x)
                    result.Add(new Cuboid(this.minPoint, new Point3D(other.minPoint.x-1, this.maxPoint.y, this.maxPoint.z)));

                if (this.maxPoint.x > other.maxPoint.x)
                    result.Add(new Cuboid(new Point3D(other.maxPoint.x + 1, this.minPoint.y, this.minPoint.z), this.maxPoint));
                
                if (this.minPoint.y < other.minPoint.y)
                    result.Add(new Cuboid(new Point3D(Mathf.Max(other.minPoint.x, this.minPoint.x), this.minPoint.y, this.minPoint.z),
                                        new Point3D(Mathf.Min(other.maxPoint.x, this.maxPoint.x), other.minPoint.y - 1, this.maxPoint.z)));

                if (this.maxPoint.y > other.maxPoint.y)
                    result.Add(new Cuboid(new Point3D(Mathf.Max(other.minPoint.x, this.minPoint.x), other.maxPoint.y + 1, this.minPoint.z),
                                        new Point3D(Mathf.Min(other.maxPoint.x, this.maxPoint.x), this.maxPoint.y, this.maxPoint.z)));

                if (this.minPoint.z < other.minPoint.z)
                    result.Add(new Cuboid(new Point3D(Mathf.Max(other.minPoint.x, this.minPoint.x), Mathf.Max(other.minPoint.y, this.minPoint.y), this.minPoint.z),
                                        new Point3D(Mathf.Min(other.maxPoint.x, this.maxPoint.x), Mathf.Min(other.maxPoint.y, this.maxPoint.y), other.minPoint.z -1)));

                if (this.maxPoint.z > other.maxPoint.z)
                    result.Add(new Cuboid(new Point3D(Mathf.Max(other.minPoint.x, this.minPoint.x), Mathf.Max(other.minPoint.y, this.minPoint.y), other.maxPoint.z + 1),
                                         new Point3D(Mathf.Min(other.maxPoint.x, this.maxPoint.x), Mathf.Min(other.maxPoint.y, this.maxPoint.y), this.maxPoint.z)));
            }

            if (debug)
            {
                Debug.Log(Tools.writeOffset(offset) + "Substracted cubes of " + this.ToString() + " by " + other.ToString() + " result : ");
                Debug.Log(Tools.writeOffset(offset) + " => count = " + result.Count + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, result.ConvertAll<string>(x => x.ToString())));
            }

            return result;
        }

        public override string ToString()
        {
            return this.minPoint.ToString() + " to " + this.maxPoint.ToString();
        }
    }

    class Point3D
    {
        public int x;
        public int y;
        public int z;

        public Point3D(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }

        public override string ToString() { return "(" + x + ", " + y + ", " + z + ")"; }
    }
}
