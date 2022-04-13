using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_19 : DayScript2021
{
    protected override string part_1()
    {
        StartCoroutine(coPart_1());
        return "In Progress";

    }

    IEnumerator coPart_1()
    { 
        List<Scanner> scans = new List<Scanner>();
        foreach (string scanBloc in _input.Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            scans.Add(new Scanner(scanBloc));
        }

        yield return new WaitForEndOfFrame();

        // take 1st element as base (as if it was on 0,0,0)
        Scanner baseScan = scans[0];

        List<Scanner> unmatchedScans = new List<Scanner>(scans);
        unmatchedScans.RemoveAt(0);

        int safetyCount = unmatchedScans.Count;
        do
        {
            Debug.LogWarning("Starting loop " + safetyCount + " with " + unmatchedScans.Count + " left" + " | BaseScan has ** " + baseScan.relativeSondes.Count + " ** for now" );
            // try to make match a scan with baseScan we already have
            List<Scanner> tmp = new List<Scanner>();
            foreach (Scanner scan in unmatchedScans)
            {
                yield return new WaitForEndOfFrame();
                if (!baseScan.Match(scan))  // we have a match
                {
                    Debug.Log("No match found for scanner " + scan.id);
                    tmp.Add(scan);
                }                    
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            unmatchedScans = new List<Scanner>(tmp);
            safetyCount--;
        } while (unmatchedScans.Count > 0 && safetyCount > 0);


        Debug.LogWarning(baseScan.relativeSondes.Count.ToString());
        //return baseScan.relativeSondes.Count.ToString();
    }

    protected override string part_2()
    {
        List<Sonde> sonds = new List<Sonde>()
        {
            new Sonde(0,0,0),
            new Sonde(-1384, -51, -28),
            new Sonde(1068, -28, 46),
            new Sonde(2225, -113, 122),
            new Sonde(2229, 1227, -1),
            new Sonde(-1324, -1197, 69),
            new Sonde(-6, 1158, -3),
            new Sonde(-86, 1126, 1316),
            new Sonde(1070, -154, -1168),
            new Sonde(-2459, 20, -28),
            new Sonde(1079, 1050, 25),
            new Sonde(-130, 2317, 1215),
            new Sonde(2256, -1265, 80),
            new Sonde(1057, -1345, -11),
            new Sonde(-2423, -82, 1293),
            new Sonde(-115, 2420, 57),
            new Sonde(-1369, -2427, 131),
            new Sonde(-54, 1154, -1091),
            new Sonde(-1203, -1282, -1192),
            new Sonde(-2423, 1137, 148),
            new Sonde(-1247, 2338, -35),
            new Sonde(-1211, 2280, 1298),
            new Sonde(2379, -1302, -1231),
            new Sonde (-3670, 1047, 5),
            new Sonde(-1348, 3620, 92),
            new Sonde(-1208, 3526, -1199),
            new Sonde(-1211, 4725, -29),
            new Sonde (-2498, 4808, 99)
        };

        int maxDist = 0;
        for (int i=0; i< sonds.Count-1; i++)
        {
            for (int j=i+1; j < sonds.Count; j++)
            {
                maxDist = Mathf.Max(maxDist, sonds[i].GetDistance(sonds[j]));
            }
        }

        return maxDist.ToString();
    }


    public class Scanner
    {
        public int id;
        public List<Sonde> relativeSondes;

        public Scanner(int p_id, List<Sonde> p_sondes)
        {
            this.id = p_id;
            this.relativeSondes = new List<Sonde>(p_sondes);
        }

        public Scanner(string input)
        {
            Debug.Log(input);
            this.id = int.Parse(input.Split('\n')[0].Split(' ')[2]);
            this.relativeSondes = new List<Sonde>();
            foreach (string sonde in input.Substring(input.IndexOf('\n')+1).Split('\n'))
            {
                relativeSondes.Add(new Sonde(sonde));
            }
        }

        public override string ToString()
        {
            string log = "Scanner ID : " + this.id + " | Has " + this.relativeSondes.Count + " beacons " + System.Environment.NewLine;
            foreach (Sonde sonde in relativeSondes)
            {
                log += sonde.ToString() + System.Environment.NewLine;
            }
            return log;
        }

        public bool Match(Scanner other, bool debug = false)
        {
            //if (debug)
                Debug.Log("Finding match with scan " + other.id);

            List<Sonde> result = new List<Sonde>();            

            List<Sonde> otherBeacons = new List<Sonde>(other.relativeSondes);

            List<List<Sonde>> allRotationBeacons = Sonde.GetAllRotations(other.relativeSondes);
            //int index = 0;
            foreach (List<Sonde> otherSondes in allRotationBeacons)
            {
                //string tmpLog = "otherSondes count : " + otherSondes.Count + "  (index : " + index + ") |  ";
                //otherSondes.ForEach(x => tmpLog += x.ToString() + " /|/ ");
                //if (debug)
                //    Debug.Log(tmpLog);

                for (int myCnt = 0; myCnt < relativeSondes.Count -10; myCnt++)
                {
                    for (int othCnt = 0; othCnt < otherSondes.Count -10; othCnt++)
                    {
                        // we try to make match beacon myCnt with beacon othCnt of the other scan and we try to look if we find 12 beacons that matches.
                        Sonde diff = otherSondes[othCnt].GetDiff(this.relativeSondes[myCnt]);

                        List<Sonde> matchingSondes = GetMatchingBeacons(new List<Sonde>(this.relativeSondes), new List<Sonde>(otherSondes), diff, debug);
                        if (matchingSondes.Count > 0)
                        {
                            Debug.Log("Match found for beacons " + myCnt + this.relativeSondes[myCnt].ToString() + " and " + othCnt + otherSondes[othCnt].ToString() + " , diff is : " + diff.ToString());
                            foreach (Sonde s in matchingSondes)
                            {
                                if (!this.relativeSondes.Contains(s))
                                    this.relativeSondes.Add(s);

                            }

                            if (debug)
                            {
                                //Debug.Log(" __ " + log);
                                Debug.Log("** " + this.ToString());
                            }
                            
                            return true;
                        }
                    }
                }

                //index++;

            }

            if (debug)
                Debug.Log("No match found between base and scanner " + other.id);


            return false;
        }

        public static List<Sonde> GetMatchingBeacons(List<Sonde> a_beacons, List<Sonde> b_beacons, Sonde diff, bool debug = false)
        {
            //string log = "Looking for match with diff " + diff.ToString() + System.Environment.NewLine;
            List<Sonde> result = new List<Sonde>();

            List<KeyValuePair<Sonde, Sonde>> matchingPairs = new List<KeyValuePair<Sonde, Sonde>>();
            foreach (Sonde b_sond in b_beacons)
            {
                //log += b_sond.Add(diff).ToString() + System.Environment.NewLine;
                Sonde res = a_beacons.Find(x => x.Equals(b_sond.Add(diff)));
                if (res != null)
                {
                    matchingPairs.Add(new KeyValuePair<Sonde, Sonde>(b_sond, res));
                    a_beacons.Remove(res);
                    //log += "Match found between beacons " + b_sond.ToString() + " and " + res.ToString() + " |  a count is " + a_beacons.Count + System.Environment.NewLine;
                }
            }

            if (matchingPairs.Count >= 12)
            {
                //if (debug)
                //{
                //    log += "We found some correspondances ! " + matchingPairs.Count + System.Environment.NewLine;
                //    matchingPairs.ForEach(x => log += x.Key.ToString() + "\t | \t" + x.Value.ToString() + System.Environment.NewLine);
                //    Debug.Log(log);
                //}

                foreach (Sonde b_sond in b_beacons)
                    result.Add(b_sond.Add(diff));
            }

            //if (debug)
            //    Debug.Log(log);

            return result;
        }
    }

    public class Sonde
    {
        public int x;
        public int y;
        public int z;

        public Sonde(int p_x, int p_y, int p_z)
        {
            this.x = p_x;
            this.y = p_y;
            this.z = p_z;
        }

        public Sonde(string input)
        {
            string[] coords = input.Split(',');
            if (coords.Length != 3)
            {
                Debug.LogError("Coordonnates for beacons are not in good format : " + input);
                return;
            }

            this.x = int.Parse(coords[0]);
            this.y = int.Parse(coords[1]);
            this.z = int.Parse(coords[2]);
        }

        public Sonde(Sonde copy)
        {
            this.x = copy.x;
            this.y = copy.y;
            this.z = copy.z;
        }

        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return base.Equals(obj);

            Sonde sonde = (Sonde)obj;
            return this.x == sonde.x && this.y == sonde.y && this.z == sonde.z;
        }
        public Sonde GetDiff(Sonde other)
        {
            return new Sonde(other.x - this.x, other.y - this.y, other.z - this.z);
        }

        public Sonde Add(Sonde adder)
        {
            return new Sonde(this.x + adder.x, this.y + adder.y, this.z + adder.z);
        }

        public int GetDistance(Sonde other)
        {
            return Mathf.Abs(other.x - this.x) + Mathf.Abs(other.y - this.y) + Mathf.Abs(other.z - this.z);
        }

        public static List<List<Sonde>> GetAllRotations(List<Sonde> sondes)
        {
            List<List<Sonde>> result = new List<List<Sonde>>();
            for (int i=0; i <24; i++) { result.Add(new List<Sonde>()); }
            //result[0] = new List<Sonde>(sondes);

            foreach (Sonde s in sondes)
            {
                result[0].Add(new Sonde(s.x, s.y, s.z));
                result[1].Add(new Sonde(s.x, -s.y, -s.z));
                result[2].Add(new Sonde(s.x, s.z, -s.y));
                result[3].Add(new Sonde(s.x, -s.z, s.y));


                result[4].Add(new Sonde(s.y, -s.x, s.z));
                result[5].Add(new Sonde(s.y, s.z, s.x));
                result[6].Add(new Sonde(s.y, s.x, -s.z));
                result[7].Add(new Sonde(s.y, -s.z, -s.x));
                
                result[8].Add(new Sonde(s.z, s.y, -s.x));
                result[9].Add(new Sonde(s.z, -s.y, s.x));
                result[10].Add(new Sonde(s.z, s.x, s.y));
                result[11].Add(new Sonde(s.z, -s.x, -s.y));


                result[12].Add(new Sonde(-s.x, s.y, -s.z));
                result[13].Add(new Sonde(-s.x, -s.y, s.z));
                result[14].Add(new Sonde(-s.x, s.z, s.y));
                result[15].Add(new Sonde(-s.x, -s.z, -s.y));

                result[16].Add(new Sonde(-s.y, s.x, s.z));
                result[17].Add(new Sonde(-s.y, s.z, -s.x));
                result[18].Add(new Sonde(-s.y, -s.x, -s.z));
                result[19].Add(new Sonde(-s.y, -s.z, s.x));

                result[20].Add(new Sonde(-s.z, s.y, s.x));
                result[21].Add(new Sonde(-s.z, s.x, -s.y));
                result[22].Add(new Sonde(-s.z, -s.y, -s.x));
                result[23].Add(new Sonde(-s.z, -s.x, s.y));
            }

            return result;
        }
    }
}
