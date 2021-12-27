using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_18 : DayScript2021
{
    protected override string part_1()
    {
        SnailfishNumber result = null;
        foreach (string line in _input.Split('\n'))
        {
            if (result == null)
                result = new SnailfishNumber(line);
            else
                result = result.Add(new SnailfishNumber(line));
        }

        return result.Evaluate().ToString();
    }

    protected override string part_2()
    {
        long result = 0;

        string[] numbers = _input.Split('\n');
        List<long> magnitudes = new List<long>();
        for (int index = 0; index < numbers.Length; index++)
        { 
            for (int i=0; i<numbers.Length; i++)
            {
                if (i == index)
                    continue;


                SnailfishNumber current = new SnailfishNumber(numbers[index]);
                SnailfishNumber adder = new SnailfishNumber(numbers[i]);
                //Debug.Log("Testing " + current.ToString() + " and " + adder.ToString());
                SnailfishNumber addition =  current.Add(new SnailfishNumber(numbers[i]));
                long res = addition.Evaluate();
                magnitudes.Add(res);
                //Debug.Log(" -> addition is " + addition.ToString() + " with magnitude of " + res);
                if (res > result)
                {
                    //Debug.LogWarning("Found new highest eval for numbers " + current.ToString() + " and " + adder.ToString() + " => " + res);
                    result = res;
                } 
            }
        }

        return result.ToString();
    }

    class SnailfishNumber
    {
        public bool isSimplePair {  
            get {
                if (leftPair != null || rightPair != null) 
                    return false;
                
                return true;
            }
        }
        public long leftInt, rightInt;
        public SnailfishNumber leftPair, rightPair;
        
        public SnailfishNumber parent;
        public bool isLeftChild;

        public SnailfishNumber()
        {
            leftInt = 0; rightInt = 0; leftPair = null; rightPair = null; parent = null; isLeftChild = false;
        }

        public SnailfishNumber(SnailfishNumber copy)
        {
            this.leftInt = copy.leftInt;
            this.rightInt = copy.rightInt;
            
            this.parent = copy.parent;
            this.isLeftChild = copy.isLeftChild;

            this.leftPair = null;
            this.rightPair = null;

            if (copy.leftPair != null)
                this.leftPair = new SnailfishNumber(copy.leftPair);
            if (copy.rightPair != null)
                this.rightPair = new SnailfishNumber(copy.rightPair);
        }

        public SnailfishNumber(string input, SnailfishNumber pParent = null, bool isLeft = false)
        {
            //Debug.Log("parsing snailfish number with input : '" + input + "'");
            this.parent = pParent;
            this.isLeftChild = isLeft;

            input = input.Substring(1, input.Length - 2);   // remove exteriors brackets
            int pos = 1;

            if (input[0] == '[')    //subPair
            {
                int counter = 1;
                while (counter != 0 && pos < input.Length)
                {
                    if (input[pos] == '[')
                        counter++;
                    else if (input[pos] == ']')
                        counter--;

                    pos++;
                }
                leftPair = new SnailfishNumber(input.Substring(0, pos), this, true);

                if (input[pos] != ',')
                    Debug.LogError("Error in parse : " + input[pos] + " / " + input.Substring(0, pos - 1));
            }
            else
            {
                pos = input.IndexOf(',');
                leftInt = int.Parse(input.Substring(0, pos));
            }

            if (input[pos + 1] == '[')
                rightPair = new SnailfishNumber(input.Substring(pos + 1), this, false);
            else
                rightInt = int.Parse(input.Substring(pos + 1));
        }

        public override string ToString()
        {
            return "[" + (this.leftPair != null ? this.leftPair.ToString() : this.leftInt.ToString()) + ", " + (this.rightPair != null ? this.rightPair.ToString() : this.rightInt.ToString()) + "]";
        }

        public long Evaluate(int offset = 0, bool isDebug = false)
        {
            long result = 0;
            if (this.leftPair != null)
            {
                if (isDebug)
                    Debug.Log(Tools.writeOffset(offset) + "Evaluating : " + this.leftPair.ToString());
                this.leftInt = this.leftPair.Evaluate(offset+1, isDebug);
            }
            if (this.rightPair != null)
            {
                if (isDebug)
                    Debug.Log(Tools.writeOffset(offset) + "Evaluating : " + this.rightPair.ToString());
                this.rightInt = this.rightPair.Evaluate(offset+1, isDebug);
            }

            result += 3 * this.leftInt;
            result += 2 * this.rightInt;

            if (isDebug)
                Debug.Log(Tools.writeOffset(offset) + "Then " + this.ToString() + " is evaluated as [" + this.leftInt + ", " + this.rightInt + "] with result " + result);

            return result;
        }

        public SnailfishNumber Add(SnailfishNumber adder, bool isDebug = false)
        {
            SnailfishNumber result = new SnailfishNumber();
            this.parent = result;
            this.isLeftChild = true;

            adder.parent = result;
            adder.isLeftChild = false;

            result.leftPair = this;
            result.rightPair = adder;

            if (isDebug)
            {
                Debug.Log("After Addition, number is : " + result.ToString());
                Debug.Log("Starting Reducing");
            }
            int safetyCount = 1;
            while (result.Reduce(0) /*&& safetyCount < 10000*/)
            {
                if (isDebug)
                    Debug.Log("Reducing " + safetyCount + " => " + result.ToString());

                safetyCount++;               
            }
            if (isDebug)
                Debug.Log("End Reducing, reduced number is : " + result.ToString());

            return result;
        }

        public bool Reduce(int depth, bool onlyExplode = false, bool isDebug = false)
        {
            bool hasReduced = false;
            if (isSimplePair && depth >= 4)
            {
                if (isDebug)
                    Debug.Log("Exploding " + this.ToString());
                parent.Explode(isLeftChild);
                return true;
            }

            if (leftPair != null && !hasReduced)   // try to explode left pair
            {
                 hasReduced |= this.leftPair.Reduce(depth + 1, true);
            }

            if (rightPair != null && !hasReduced)
            {
                hasReduced |= this.rightPair.Reduce(depth + 1, true);
            }

            if (onlyExplode)
                return hasReduced;

            // no explosion, look for a split
            if (this.leftPair == null && !hasReduced)
            {
                if (this.leftInt > 9)
                {
                    hasReduced = true;
                    this.leftPair = new SnailfishNumber() { parent = this, isLeftChild = true, leftInt = Mathf.FloorToInt((float)this.leftInt / 2f), rightInt = Mathf.CeilToInt((float)this.leftInt / 2f) };
                    //Debug.Log("Splitting left child '" + this.leftInt + "' in pair " + this.leftPair.ToString() + " (" + this.leftPair.type.ToString() + ")");
                    this.leftInt = 0;
                }
            }
            else if (leftPair != null && !hasReduced)   // try to split in left pair
            {
                hasReduced |= this.leftPair.Reduce(depth + 1, false);
            }


            if (this.rightPair == null && !hasReduced)
            {
                if (this.rightInt > 9)
                {
                    hasReduced = true;
                    this.rightPair = new SnailfishNumber() { parent = this, isLeftChild = false, leftInt = Mathf.FloorToInt((float)this.rightInt / 2f), rightInt = Mathf.CeilToInt((float)this.rightInt / 2f) };
                    //Debug.Log("Splitting right child '" + this.rightInt + "' in pair " + this.rightPair.ToString() + " (" + this.rightPair.type.ToString() + ")");
                    this.rightInt = 0;                    
                }
            }
            else if (rightPair != null && !hasReduced)
            {
                hasReduced |= this.rightPair.Reduce(depth + 1, false);
            }


            return hasReduced;
        }

        public void Explode(bool isLeft)
        {
            long leftVal = isLeft ? this.leftPair.leftInt : this.rightPair.leftInt;
            long rightVal = isLeft ? this.leftPair.rightInt : this.rightPair.rightInt;

            if (isLeft)
            {
                leftPair = null;
                leftInt = 0;
                if (rightPair != null)
                    rightPair.AddToMostLeftAfterInt(rightVal, null);
                else
                    rightInt += rightVal;

                if (parent != null)
                    parent.AddToMostRightBeforeInt(leftVal, this.isLeftChild);
            }
            else
            {
                rightPair = null;
                rightInt = 0;
                if (leftPair != null)
                    leftPair.AddToMostRightBeforeInt(leftVal, null);
                else
                    leftInt += leftVal;

                if (parent != null)
                    parent.AddToMostLeftAfterInt(rightVal, this.isLeftChild);
            }
        }

        public void AddToMostLeftAfterInt(long value, bool? isLeft)
        {
            if (isLeft == null)     // call to a child pair
            {
                if (this.leftPair != null)
                    this.leftPair.AddToMostLeftAfterInt(value, null);
                else
                    this.leftInt += value;
            }
            else
            {           // call to parent, need to know where digging
                if (isLeft.Value)
                {
                    if (this.rightPair != null)
                        this.rightPair.AddToMostLeftAfterInt(value, null);
                    else
                        this.rightInt += value;
                }
                else
                {
                    if (parent != null)
                        parent.AddToMostLeftAfterInt(value, this.isLeftChild);
                }
            }
        }

        public void AddToMostRightBeforeInt(long value, bool? isLeft)
        {
            if (isLeft == null)
            {
                if (this.rightPair != null)
                    this.rightPair.AddToMostRightBeforeInt(value, null);
                else
                    this.rightInt += value;
            }
            else
            {           // call to parent, need to know where digging
                if (isLeft.Value)
                {
                    if (parent != null)
                        parent.AddToMostRightBeforeInt(value, this.isLeftChild);
                }
                else
                {
                    if (this.leftPair != null)
                        this.leftPair.AddToMostRightBeforeInt(value, null);
                    else
                        this.leftInt += value;
                }
            }
        }
    }
}
