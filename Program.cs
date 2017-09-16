using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EquationCalc
{
    class Program
    {
        public static List<string> leftArray { get;  set; }
        public static List<string> rightArray { get;  set; }
        public static int quadraticEquations = 0;

        static void Main(string[] args)
        {
            /*
             3 X + 12/ 4= 3^2  +2*2    - 2 - 2*2
             3 X + 12/ 4= (8/2*2+4)  +2*2    - 2 - 2*4
             3 X + 12/ 4= (8/2*2+4X)  +2*2    - 2 - 2*4
             3 X + (12/ 4)3= - 2 - 2*4
             3 X + (12/ 4)3= - 2 - (2*4)+(2*2)
            */
            string readStr = "";
            string leftStr = "";
            string rightStr = "";
            while (readStr != "exit")
            {
                readStr = Console.ReadLine();
                readStr=readStr.Replace(" ",string.Empty);
                Console.WriteLine(readStr);
                List<string> divideLeftRight = readStr.Split('=').ToList();

                if (divideLeftRight.Count == 2)
                {
                    leftStr=divideLeftRight.ElementAt<string>(0);
                    rightStr = divideLeftRight.ElementAt<string>(1);
                    Console.WriteLine("Left-------------------------");
                    leftArray= parserFunction(leftStr);
                    foreach (string left in leftArray)
                    {
                        Console.WriteLine(left);
                    }
                    Console.WriteLine("Right-------------------------");
                    rightArray = parserFunction(rightStr);
                    foreach (string Right in rightArray)
                    {
                        Console.WriteLine(Right);
                    }
                    Console.WriteLine("result------------------------");
                    calcFunction(leftArray, rightArray);
                }
                else
                {
                    Console.WriteLine("Something wrong at = ");

                }
            }
           
        }
        /*
         * 
         */
        static void calcFunction(List<string> leftArray, List<string> rightArray)
        {
            if (quadraticEquations == 1)
            {
                quadraticEquationsFunction(leftArray, rightArray);
            }
            else
            {
                decisionMakingFunction(rightArray, "right");
                decisionMakingFunction(leftArray, "left");
            }
        }
        /*
         * 
         */
        static string decisionMakingFunction(List<string> tempArray,string side)
        {
            int stopFlag = 1;
            if (side == "right")
            {
                tempArray = rightArray;
            }
            else if (side == "left")
            {
                tempArray = leftArray;
            }
            while (stopFlag != 0)
            {
                if (tempArray.Contains<string>("(") && tempArray.Contains<string>(")"))
                {
                    
                    int index = 0;
                    int startindex = tempArray.IndexOf("(");
                    int endindex = tempArray.IndexOf(")");
                    int indexGap = endindex - startindex;
                    string multiplyVariable = "";
                    int multiplyVariableFlag = 0;
                    List<string> total = new List<string>();
                    List<string> tempList = new List<string>();
                    if (startindex>1 && tempArray[startindex - 1].ToString() == "*")
                    {
                        if (digitDetectFunction(tempArray[startindex - 2].ToString())) { multiplyVariable = tempArray[startindex - 2].ToString(); }
                    }

                    for (int i = startindex + 1; i < endindex; i++)
                    {
                        tempList.Add(tempArray[i].ToString());
                    }
                    if (xdetectInListFunction(tempArray))
                    {
                        total = calculationFunction(tempList, 1, side);
                    }
                    else
                    {
                        total = calculationFunction(tempList, 0, side);
                    }
                    Console.WriteLine("(.................................)");
                    if (multiplyVariable!="")
                    {
                        multiplyVariableFlag = 1;
                        List<string>temptotal = new List<string>();
                        foreach (string tot in total)
                        {
                            if (digitDetectFunction(tot))
                            {
                                int multiplyTotal = Convert.ToInt32(multiplyVariable) * Convert.ToInt32(tot);
                                temptotal.Add(multiplyTotal.ToString());
                            }
                            else if (xdetectFunction(tot))
                            {
                                int multiplyTotal = 0;
                                string temp = tot;
                                temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                if (temp != "")
                                {
                                    multiplyTotal = Convert.ToInt32(multiplyVariable) * Convert.ToInt32(temp);
                                }
                                else
                                {
                                    multiplyTotal = Convert.ToInt32(multiplyVariable);
                                }
                                if (multiplyTotal != 0 || multiplyTotal != 1)
                                {
                                    temptotal.Add(multiplyTotal+"x");
                                }
                                else
                                {
                                    temptotal.Add(tot);
                                }
                            }
                            else
                            {
                                temptotal.Add(tot);
                            }

                        }
                        total = temptotal;
                    }

                    foreach (string tot in total)
                    {
                        Console.WriteLine(tot);

                    }
                    if (total.Count == 3)
                    {
                        if (startindex != 0 && digitDetectFunction(tempArray[startindex - 1].ToString()))
                        {

                        }
                        else if(multiplyVariableFlag == 1)
                        {
                            tempArray.Insert(startindex-2, total[2].ToString());
                            tempArray.Insert(startindex-2, total[1].ToString());
                            tempArray.Insert(startindex-2, total[0].ToString());
                            tempArray.RemoveRange(startindex + 1, indexGap + 3);
                        }
                        else
                        {
                            tempArray.Insert(startindex, total[0].ToString());
                            tempArray.Insert(startindex, total[1].ToString());
                            tempArray.Insert(startindex, total[2].ToString());
                            tempArray.RemoveRange(startindex + 3, indexGap + 1);
                        }
                    }
                    else if (multiplyVariableFlag == 1 && total.Count == 1)
                    {
                        tempArray.Insert(startindex-2, total[0].ToString());
                        tempArray.RemoveRange(startindex -1, indexGap + 3);
                    }
                    else if (total.Count == 1)
                    {
                        tempArray.Insert(startindex, total[0].ToString());
                        tempArray.RemoveRange(startindex + 1, indexGap + 1);
                    }

                }
                else if (side == "right" && xdetectInListFunction(tempArray))
                {
                    if (xdetectInListFunction(leftArray) == false)
                    {
                        List<string> temporaryleftArray = new List<string>();
                        List<string> temporaryrightArray = new List<string>();
                        temporaryleftArray = leftArray;
                        temporaryrightArray = rightArray;
                        leftArray = temporaryrightArray;
                        rightArray = temporaryleftArray;
                        tempArray = rightArray;
                    }
                    else
                    { 
                        int index = 0;
                        int flag = 0;
                        string value = "";
                        List<string> total = new List<string>();
                        total = calculationFunction(tempArray, 1, side);
                        foreach (string aChar in total)
                        {
                            if (xdetectFunction(aChar))
                            {
                                index = total.IndexOf(aChar);
                                flag = 1;
                                value = aChar;
                            }
                        }
                        if (index == 0 && flag == 1 && value != "" && side == "right")
                        {

                            if (total.Count() == 1)
                            {
                                leftArray.Add("-");
                                leftArray.Add(value);
                                if (xdetectFunction(total[index].ToString()))
                                {
                                    total.RemoveRange(index, 1);
                                    total.Add("0");
                                }
                            }
                            else
                            {
                                if (total[index + 1].ToString() == "+")
                                {
                                    leftArray.Add("-");
                                    leftArray.Add(value);
                                    total.RemoveRange(index, 2);
                                }
                                else if (total[index + 1].ToString() == "-")
                                {
                                    leftArray.Add("-");
                                    leftArray.Add(value);
                                    string valueString = total[index + 2].ToString();
                                    int valueNumber = Convert.ToInt32(valueString);
                                    valueNumber = valueNumber * (-1);
                                    total[index + 2] = valueNumber.ToString();
                                    total.RemoveRange(index, 2);
                                }
                                else if (total[index + 1].ToString() == "/" || total[index + 1].ToString() == "*")
                                {
                                    List<string> temporaryleftArray = new List<string>();
                                    List<string> temporaryrightArray = new List<string>();
                                    temporaryleftArray = leftArray;
                                    temporaryrightArray = rightArray;
                                    leftArray = temporaryrightArray;
                                    rightArray = temporaryleftArray;
                                    tempArray = rightArray;
                                }
                            }
                        }
                        else if (index > 0 && flag == 1 && value != "" && side == "right")
                        {
                            string sign = changeSign(total[index - 1].ToString());
                            leftArray.Add(sign);
                            leftArray.Add(value);
                            total.RemoveRange(index - 1, 2);
                        }
                    }
                }
                else if (side == "left" && xdetectInListFunction(tempArray))
                {
                    int index = 0;
                    int rightSideResult = 0;
                    int sum = 0;
                    List<int> xindexArray = new List<int>();
                    List<int> digitIndexArray = new List<int>();
                    int flag = 0;
                    int dflag = 0;
                    string value = "";
                    List<string> total = new List<string>();
                    total = calculationFunction(tempArray, 1, side);
                    foreach (string aChar in total)
                    {
                        if (xdetectFunction(aChar))
                        {
                            index = total.IndexOf(aChar);
                            xindexArray.Add(index);
                            flag = 1;
                            value = aChar;
                        }
                        if (digitDetectFunction(aChar))
                        {
                            index = total.IndexOf(aChar);
                            digitIndexArray.Add(index);
                            dflag = 1;
                        }
                    }
                    if (xindexArray.Count > 0 && flag == 1 && side == "left")
                    {
                        //final count and result
                        foreach (int xIndex in xindexArray)
                        {
                            if (xIndex == 0)
                            {
                                string temp = total[xIndex].ToString();
                                temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                if (temp != "") { sum = sum + Convert.ToInt32(temp); }
                                else { sum = sum + 1; }
                            }
                            else if (xIndex > 0)
                            {
                                if (total[xIndex - 1].ToString() == "/")
                                {
                                    if (xIndex - 2 == 0 && xdetectFunction(total[xIndex].ToString()))
                                    {
                                        string temp = total[xIndex].ToString();
                                        temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                        if (temp != "") { sum = sum / Convert.ToInt32(temp); }
                                        else { sum = sum / 1; }
                                    }
                                    else if (xdetectFunction(total[xIndex-2].ToString()) && xdetectFunction(total[xIndex].ToString()))
                                    {
                                        string temp1 = total[xIndex-2].ToString();
                                        string temp2 = total[xIndex].ToString();
                                        temp1 = new string(temp1.Where(c => Char.IsDigit(c)).ToArray());
                                        temp2 = new string(temp2.Where(c => Char.IsDigit(c)).ToArray());
                                        if (temp1 == "" ) { temp1 = "1"; }
                                        if (temp2 == "") { temp2 = "1"; }
                                        sum = sum + Convert.ToInt32(temp1)/ Convert.ToInt32(temp2);
                                    }
                                    
                                }
                                else if (total[xIndex-1].ToString()=="+")
                                {
                                    string temp = total[xIndex].ToString();
                                    temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                    if (temp != "") { sum = sum + Convert.ToInt32(temp); }
                                    else { sum = sum + 1; }
                                }
                                else if (total[xIndex - 1].ToString() == "-")
                                {
                                    string temp = total[xIndex].ToString();
                                    temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                    if (temp != "") { sum = sum - Convert.ToInt32(temp); }
                                    else { sum = sum -1; }
                                }
                            }
                        }                      
                            
                        rightSideResult =  Convert.ToInt32(rightArray[0].ToString());
                        foreach (int digitIndex in digitIndexArray)
                        {
                            if (digitIndex == 0)
                            {
                                string temp = total[digitIndex].ToString();
                                rightSideResult = rightSideResult - Convert.ToInt32(temp);

                            }
                            else if (digitIndex > 0)
                            {
                                if (total[digitIndex - 1].ToString() == "+")
                                {
                                    string temp = total[digitIndex].ToString();
                                    rightSideResult = rightSideResult - Convert.ToInt32(temp);
                                }
                                else if (total[digitIndex - 1].ToString() == "-")
                                {
                                    string temp = total[digitIndex].ToString();
                                    rightSideResult = rightSideResult + Convert.ToInt32(temp);
                                }
                                else if (total[digitIndex - 1].ToString() == "/")
                                {
                                    string temp = total[digitIndex].ToString();
                                    rightSideResult = rightSideResult * Convert.ToInt32(temp);
                                }
                                else if (total[digitIndex - 1].ToString() == "*")
                                {
                                    string temp = total[digitIndex].ToString();
                                    rightSideResult = rightSideResult / Convert.ToInt32(temp);
                                }
                            }
                        }
                        if (sum != 0)
                        {
                            rightSideResult = rightSideResult / sum;
                        }
                    }
                    Console.WriteLine("result of x ="+ rightSideResult);
                    break;
                }
                else if (side == "qleft" && xdetectInListFunction(tempArray))
                {
                    int index = 0;
                    int digitTotalResult = 0;
                    int sum = 0;
                    int quardicXSum = 0;
                    double positiveResult = 0;
                    double negativeResult = 0;
                    List<int> xindexArray = new List<int>();
                    List<int> quardicXindexArray = new List<int>();
                    List<int> digitIndexArray = new List<int>();
                    int flag = 0;
                    int dflag = 0;
                    int quardicXflag = 0;
                    string value = "";
                    int i = 0;
                    List<string> total = new List<string>();
                    total = calculationFunction(tempArray, 1, side);
                    int totalCount = total.Count();
                    foreach (string aChar in total)
                    {
                        if (xdetectFunction(aChar))
                        {
                            if (i >= 0 && i < totalCount - 2)
                            {
                                if (total[i + 1].ToString() == "^" && digitDetectFunction(total[i + 2].ToString()))
                                {
                                    index = total.IndexOf(aChar);
                                    quardicXindexArray.Add(i);
                                    quardicXflag = 1;
                                    value = aChar;
                                }
                                else
                                {
                                    index = total.IndexOf(aChar);
                                    xindexArray.Add(i);
                                    flag = 1;
                                    value = aChar;
                                }
                            }
                            else
                            {
                                index = total.IndexOf(aChar);
                                xindexArray.Add(i);
                                flag = 1;
                                value = aChar;
                            }
                            
                        }
                        if (digitDetectFunction(aChar))
                        {
                            if (i > 1 && i < totalCount)
                            {
                                if (total[i - 1].ToString() == "^" && xdetectFunction(total[i - 2].ToString()))
                                {
                                    //skip
                                }
                                else
                                {
                                    index = total.IndexOf(aChar);
                                    digitIndexArray.Add(i);
                                    dflag = 1;
                                }
                            }
                            else
                            {
                                index = total.IndexOf(aChar);
                                digitIndexArray.Add(i);
                                dflag = 1;
                            }
                            
                        }
                        i++;
                    }
                    if (quardicXindexArray.Count > 0  && quardicXflag == 1 && side == "qleft")
                    {
                        foreach (int quardicXIndex in quardicXindexArray)
                        {
                            if (quardicXIndex == 0)
                            {
                                string temp = total[quardicXIndex].ToString();
                                temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                if (temp != "") { quardicXSum = quardicXSum + Convert.ToInt32(temp); }
                                else { quardicXSum = quardicXSum + 1; }
                            }
                            else if (quardicXIndex > 0)
                            {
                                if (total[quardicXIndex - 1].ToString() == "+")
                                {
                                    string temp = total[quardicXIndex].ToString();
                                    temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                    if (temp != "") { quardicXSum = quardicXSum + Convert.ToInt32(temp); }
                                    else { quardicXSum = quardicXSum + 1; }
                                }
                                else if (total[quardicXIndex - 1].ToString() == "-")
                                {
                                    string temp = total[quardicXIndex].ToString();
                                    temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                    if (temp != "") { quardicXSum = quardicXSum - Convert.ToInt32(temp); }
                                    else { quardicXSum = quardicXSum - 1; }
                                }

                            }
                        }
                        //final count and result
                        foreach (int xIndex in xindexArray)
                        {
                            if (xIndex == 0)
                            {
                                string temp = total[xIndex].ToString();
                                temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                if (temp != "") { sum = sum + Convert.ToInt32(temp); }
                                else { sum = sum + 1; }
                            }
                            else if (xIndex > 0)
                            {
                                if (total[xIndex - 1].ToString() == "/")
                                {
                                    if (xIndex - 2 == 0 && xdetectFunction(total[xIndex].ToString()))
                                    {
                                        string temp = total[xIndex].ToString();
                                        temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                        if (temp != "") { sum = sum / Convert.ToInt32(temp); }
                                        else { sum = sum / 1; }
                                    }
                                    else if (xdetectFunction(total[xIndex - 2].ToString()) && xdetectFunction(total[xIndex].ToString()))
                                    {
                                        string temp1 = total[xIndex - 2].ToString();
                                        string temp2 = total[xIndex].ToString();
                                        temp1 = new string(temp1.Where(c => Char.IsDigit(c)).ToArray());
                                        temp2 = new string(temp2.Where(c => Char.IsDigit(c)).ToArray());
                                        if (temp1 == "") { temp1 = "1"; }
                                        if (temp2 == "") { temp2 = "1"; }
                                        sum = sum + Convert.ToInt32(temp1) / Convert.ToInt32(temp2);
                                    }

                                }
                                else if (total[xIndex - 1].ToString() == "+")
                                {
                                    string temp = total[xIndex].ToString();
                                    temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                    if (temp != "") { sum = sum + Convert.ToInt32(temp); }
                                    else { sum = sum + 1; }
                                }
                                else if (total[xIndex - 1].ToString() == "-")
                                {
                                    string temp = total[xIndex].ToString();
                                    temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                    if (temp != "") { sum = sum - Convert.ToInt32(temp); }
                                    else { sum = sum - 1; }
                                }
                            }
                        }
                        /////////////don't need below code////////////////////////////
                        digitTotalResult = 0;
                        
                        foreach (int digitIndex in digitIndexArray)
                        {
                            if (digitIndex == 0)
                            {
                                string temp = total[digitIndex].ToString();
                                digitTotalResult = digitTotalResult + Convert.ToInt32(temp);

                            }
                            else if (digitIndex > 0)
                            {
                                if (total[digitIndex - 1].ToString() == "+")
                                {
                                    string temp = total[digitIndex].ToString();
                                    digitTotalResult = digitTotalResult + Convert.ToInt32(temp);
                                }
                                else if (total[digitIndex - 1].ToString() == "-")
                                {
                                    string temp = total[digitIndex].ToString();
                                    digitTotalResult = digitTotalResult - Convert.ToInt32(temp);
                                }
                                else if (total[digitIndex - 1].ToString() == "/")
                                {
                                    string temp = total[digitIndex].ToString();
                                    digitTotalResult = digitTotalResult / Convert.ToInt32(temp);
                                }
                                else if (total[digitIndex - 1].ToString() == "*")
                                {
                                    string temp = total[digitIndex].ToString();
                                    digitTotalResult = digitTotalResult * Convert.ToInt32(temp);
                                }
                            }
                        }

                        if (flag == 1 && quardicXflag == 1)
                        {
                            double a = quardicXSum;
                            double b = sum;
                            double c = digitTotalResult;
                            double valueForRoot = Math.Pow(b, 2) - 4 * a * c;
                            if (valueForRoot < 0)
                            {
                                valueForRoot = valueForRoot * (-1);
                            }
                            positiveResult = ((-b + Math.Sqrt(valueForRoot)) / (2 * a));
                            negativeResult = ((-b - Math.Sqrt(valueForRoot)) / (2 * a));
                            leftArray.Clear();
                            rightArray.Clear();
                            tempArray.Clear();
                        }
                        else if (flag == 0 && quardicXflag == 1)
                        {
                            double a = quardicXSum;
                            double b = sum;
                            double c = digitTotalResult;
                            double valueForRoot = Math.Pow(b, 2) - 4 * a * c;
                            if (valueForRoot < 0)
                            {
                                valueForRoot = valueForRoot * (-1);
                            }
                            positiveResult = ((-b + Math.Sqrt(valueForRoot)) / (2 * a));
                            negativeResult = ((-b - Math.Sqrt(valueForRoot)) / (2 * a));

                            leftArray.Clear();
                            rightArray.Clear();
                            tempArray.Clear();
                        }
                    }
                    Console.WriteLine("result of x =" + positiveResult + " or "+ negativeResult);
                    
                }
                else
                {
                    calculationFunction(tempArray, 0, side);
                }
                if (tempArray.Count()==1)
                {
                    stopFlag = 0;
                }
            }
            return null;
        }
        /*
         * 
         */
        static List<string> calculationFunction(List<string> tempArray,int xExists,string side){

            int stopFlag = 1;
            int priorityFlag = 0;
            int minusTracker = 0;
            List<string> priorityArray = new List<string>();
            List<string> priorityTempArray = new List<string>();
            List<int> priorityIndexArray = new List<int>();
            List<int> priorityIndexTempArray = new List<int>();
            try
            {
                int i = 0;
                foreach (string aChar in tempArray)
                {
                    if (aChar == "^")
                    {
                        priorityArray.Add("^");
                        priorityIndexArray.Add(i);
                    }
                    else if (aChar == "*")
                    {
                        priorityArray.Add("*");
                        priorityIndexArray.Add(i);
                    }
                    else if (aChar == "/")
                    {
                        priorityArray.Add("/");
                        priorityIndexArray.Add(i);
                    }
                    else if (aChar == "%")
                    {
                        priorityArray.Add("%");
                        priorityIndexArray.Add(i);
                    }
                    else if (aChar == "+")
                    {
                        priorityTempArray.Add("+");
                        priorityIndexTempArray.Add(i);
                    }
                    else if (aChar == "-")
                    {
                        priorityTempArray.Add("-");
                        priorityIndexTempArray.Add(i);
                    }
                    i++;
                }
                priorityArray.AddRange(priorityTempArray);
                priorityIndexArray.AddRange(priorityIndexTempArray);
                i = 0;
                foreach (string aChar in priorityArray)
                {
                    if (tempArray.Contains<string>("^"))
                    {
                        priorityFlag = 0;
                        int index = 0;
                        double tempTotal = 0;
                        index = tempArray.IndexOf("^");
                        string tempLeft = tempArray[index - 1].ToString();
                        string tempRight = tempArray[index + 1].ToString();
                        if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                        {
                            tempTotal = Math.Pow(Convert.ToDouble(tempLeft), Convert.ToDouble(tempRight));
                            Console.WriteLine("step--" + tempLeft + "^" + tempRight + "----" + tempTotal);
                            tempArray.Insert(index - 1, tempTotal.ToString());
                            tempArray.RemoveRange(index, 3);
                        }
                    }
                    if (tempArray.Contains<string>("%"))
                    {
                        priorityFlag = 0;
                        int index = 0;
                        double tempTotal = 0;
                        index = tempArray.IndexOf("%");
                        string tempLeft = tempArray[index - 1].ToString();
                        string tempRight = tempArray[index + 1].ToString();
                        if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                        {
                            tempTotal = Convert.ToDouble(tempLeft)% Convert.ToDouble(tempRight);
                            Console.WriteLine("step--" + tempLeft + "^" + tempRight + "----" + tempTotal);
                            tempArray.Insert(index - 1, tempTotal.ToString());
                            tempArray.RemoveRange(index, 3);
                        }
                    }
                    if (tempArray.Contains<string>("*"))
                    {
                        priorityFlag = 0;
                        int index = 0;
                        int tempTotal = 0;
                        int escapeFlag = 0;
                        index = tempArray.IndexOf("*");
                        string tempLeft = tempArray[index - 1].ToString();
                        string tempRight = tempArray[index + 1].ToString();
                        if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                        {
                            tempTotal = Convert.ToInt32(tempLeft) * Convert.ToInt32(tempRight);
                            Console.WriteLine("step--" + tempLeft + "*" + tempRight + "----" + tempTotal);
                            tempArray.Insert(index - 1, tempTotal.ToString());
                            tempArray.RemoveRange(index, 3);
                            escapeFlag = 1;
                        }
                        if (escapeFlag ==0)
                        {
                            if (index > 0 && digitDetectFunction(tempArray[index - 1].ToString()) && tempArray[index + 1].ToString() == "X" || tempArray[index + 1].ToString() == "x")
                            {
                                string tempString = tempArray[index - 1].ToString() + tempArray[index + 1].ToString();
                                Console.WriteLine("step--" + tempString + "*" + tempString + "----" + tempString);
                                tempArray.Insert(index - 1, tempString);
                                tempArray.RemoveRange(index, 3);
                            }
                        }

                    }
                    if (tempArray.Contains<string>("/"))
                    {
                        priorityFlag = 0;
                        int index = 0;
                        double tempTotal = 0;
                        index = tempArray.IndexOf("/");
                        string tempLeft = tempArray[index - 1].ToString();
                        string tempRight = tempArray[index + 1].ToString();
                        if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                        {
                            tempTotal = Convert.ToDouble(tempLeft) / Convert.ToDouble(tempRight);
                            Console.WriteLine("step--" + tempLeft + "/" + tempRight + "----" + tempTotal);
                            tempArray.Insert(index - 1, tempTotal.ToString());
                            tempArray.RemoveRange(index, 3);
                        }

                    }
                    if (tempArray.Contains<string>("+"))
                    {
                        priorityFlag = 0;
                        int index = 0;
                        double tempTotal = 0;
                        int escapeFlag = 0;
                        index = tempArray.IndexOf("+");
                        string tempLeft = tempArray[index - 1].ToString();
                        if (index>2 && tempArray[index - 2].ToString()=="-")
                        {
                            tempArray[index - 2] = "+";
                            double integerTempLeft = Convert.ToDouble(tempLeft) * (-1);
                            tempLeft = integerTempLeft.ToString();
                        }
                        if (index > 2 && (tempArray[index - 2].ToString() == "*"|| tempArray[index - 2].ToString() == "/" || tempArray[index - 2].ToString() == "%" || tempArray[index - 2].ToString() == "^"))
                        {
                            escapeFlag = 1;
                        }

                        string tempRight = tempArray[index + 1].ToString();
                        if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight) && escapeFlag == 0)
                        {
                            tempTotal = Convert.ToDouble(tempLeft) + Convert.ToDouble(tempRight);
                            Console.WriteLine("step--" + tempLeft + "+" + tempRight + "----" + tempTotal);
                            tempArray.Insert(index - 1, tempTotal.ToString());
                            tempArray.RemoveRange(index, 3);
                        }
                    }
                    if (tempArray.Contains<string>("-"))
                    {
                        priorityFlag = 0;
                        int negativeFlag = 0;
                        int index = 0;
                        double tempTotal = 0;
                        int escapeFlag = 0;
                        index = tempArray.IndexOf("-");

                        index = priorityIndexArray[i];
                        if (priorityArray[i] == "-" && minusTracker > 0)
                        {
                            index = priorityIndexArray[i];
                            index = index - 3;
                        }
                        else if(priorityArray[i]=="-")
                        {
                            index = priorityIndexArray[i];
                        }
                        string tempLeft = tempArray[index - 1].ToString();
                        string tempRight = tempArray[index + 1].ToString();
                        if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                        {
                            if (index - 1 > 0)
                            {
                                if (tempArray[index - 2].ToString() == "-")
                                {
                                    negativeFlag = 1;
                                }
                                else if (tempArray[index - 2].ToString() == "*" || tempArray[index - 2].ToString() == "/" || tempArray[index - 2].ToString() == "%" || tempArray[index - 2].ToString() == "^")
                                {
                                    escapeFlag = 1;
                                }
                            }
                            if (escapeFlag == 0)
                            {
                                if (negativeFlag == 1)
                                {
                                    tempTotal = -Convert.ToDouble(tempLeft) - Convert.ToDouble(tempRight);
                                    tempArray[index - 2] = "+";
                                }
                                else { tempTotal = Convert.ToDouble(tempLeft) - Convert.ToDouble(tempRight); }

                                ///skip
                                Console.WriteLine("step--" + tempLeft + "-" + tempRight + "----" + tempTotal);
                                tempArray.Insert(index - 1, tempTotal.ToString());
                                tempArray.RemoveRange(index, 3);
                                minusTracker += 1;
                            }
                           
                        }
                        
                    }
                    i++;
                }
                return tempArray;
            }
            catch (DivideByZeroException eDivideByZeroException)
            {

                Console.WriteLine("Error- Divided by zero . ");
                tempArray.Clear();
                leftArray.Clear();
                rightArray.Clear();
                return tempArray;
            }
            catch (IndexOutOfRangeException eIndexOutOfRangeException)
            {

                Console.WriteLine("Error- Out of Integer Range errors.");
                tempArray.Clear();
                leftArray.Clear();
                rightArray.Clear();
                return tempArray;
            }
        }
        /*
         * 
         */
        static void quadraticEquationsFunction(List<string> leftArray, List<string> rightArray)
        {
            List<string> tempArray = new List<string>();
            int i = 0;
            int escapeFlag= 0;
            foreach (string temp in rightArray)
            {
                if (temp == "-" || temp == "+")
                {
                    tempArray.Add(changeSign(temp));
                    if (i == 0) { escapeFlag = 1; }
                }
                else { tempArray.Add(temp);  }
                i++;
            }
            if (escapeFlag == 0) { leftArray.Add("-"); }
            leftArray.AddRange(tempArray);
            leftArray = quadraticEquationsSmiplificationFunction(leftArray);
            decisionMakingFunction(leftArray, "qleft");
        }
        /*
         * 
         */
        static List<string> quadraticEquationsSmiplificationFunction(List<string> leftArray)
        {
            int i = 0;
            List<int> digitIndexArray = new List<int>();
            List<string> priorityArray = new List<string>();
            foreach (string aChar in leftArray)
            {
                if (aChar == "^")
                {
                    priorityArray.Add("^");
                }
                else if (aChar == "*")
                {
                    priorityArray.Add("*");
                }
                else if (aChar == "/")
                {
                    priorityArray.Add("/");
                }
                else if (aChar == "%")
                {
                    priorityArray.Add("%");
                }
                
                i++;
            }
            foreach (string temp in priorityArray)
            {
                if (leftArray.Contains<string>("^"))
                {
                    
                    int index = 0;
                    double tempTotal = 0;
                    index = leftArray.IndexOf("^");
                    string tempLeft = leftArray[index - 1].ToString();
                    string tempRight = leftArray[index + 1].ToString();
                    if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                    {
                        tempTotal = Math.Pow(Convert.ToDouble(tempLeft), Convert.ToDouble(tempRight));
                        Console.WriteLine("step--" + tempLeft + "^" + tempRight + "----" + tempTotal);
                        leftArray.Insert(index - 1, tempTotal.ToString());
                        leftArray.RemoveRange(index, 3);
                    }
                }
                if (leftArray.Contains<string>("*"))
                {
                    
                    int index = 0;
                    int tempTotal = 0;
                    int escapeFlag = 0;
                    index = leftArray.IndexOf("*");
                    string tempLeft = leftArray[index - 1].ToString();
                    string tempRight = leftArray[index + 1].ToString();
                    if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                    {
                        tempTotal = Convert.ToInt32(tempLeft) * Convert.ToInt32(tempRight);
                        Console.WriteLine("step--" + tempLeft + "*" + tempRight + "----" + tempTotal);
                        leftArray.Insert(index - 1, tempTotal.ToString());
                        leftArray.RemoveRange(index, 3);
                        escapeFlag = 1;
                    }
                    if (escapeFlag == 0)
                    {
                        if (index > 0 && digitDetectFunction(leftArray[index - 1].ToString()) && leftArray[index + 1].ToString() == "X" || leftArray[index + 1].ToString() == "x")
                        {
                            string tempString = leftArray[index - 1].ToString() + leftArray[index + 1].ToString();
                            Console.WriteLine("step--" + tempString + "*" + tempString + "----" + tempString);
                            leftArray.Insert(index - 1, tempString);
                            leftArray.RemoveRange(index, 3);
                        }
                    }

                }
                if (leftArray.Contains<string>("/"))
                {
                    
                    int index = 0;
                    int tempTotal = 0;
                    index = leftArray.IndexOf("/");
                    string tempLeft = leftArray[index - 1].ToString();
                    string tempRight = leftArray[index + 1].ToString();
                    if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                    {
                        tempTotal = Convert.ToInt32(tempLeft) / Convert.ToInt32(tempRight);
                        Console.WriteLine("step--" + tempLeft + "/" + tempRight + "----" + tempTotal);
                        leftArray.Insert(index - 1, tempTotal.ToString());
                        leftArray.RemoveRange(index, 3);
                    }

                }
                i++;
            }
            i = 0;
            foreach (string temp in leftArray)
            {
                int totalIndex = leftArray.Count();
                if (i >= 0 && i<totalIndex)
                {
                    if (digitDetectFunction(temp))
                    {
                        if (i == 0 && (leftArray[i + 1].ToString() != "/" || leftArray[i + 1].ToString() != "*" || leftArray[i + 1].ToString() != "%" || leftArray[i + 1].ToString() != "^"))
                        {
                            digitIndexArray.Add(i);
                        }
                        else if (i == totalIndex-1 && (leftArray[i - 1].ToString() == "+" || leftArray[i - 1].ToString() == "-"))
                        {
                            digitIndexArray.Add(i);
                        }
                        else if (i > 0 && (leftArray[i - 1].ToString() == "+" || leftArray[i - 1].ToString() == "-") && (leftArray[i + 1].ToString() != "/" || leftArray[i + 1].ToString() != "*" || leftArray[i + 1].ToString() != "%" || leftArray[i + 1].ToString() != "^"))
                        {
                            digitIndexArray.Add(i);
                        }
                    }
                }
                i++;
            }
            double sum = 0;
            foreach (int index in digitIndexArray)
            {
                if (index == 0)
                {
                    sum += Convert.ToDouble(leftArray[index].ToString());
                }
                else if (index>0)
                {
                    if (leftArray[index - 1].ToString() == "+")
                    {
                        sum += Convert.ToDouble(leftArray[index].ToString());
                    }
                    else if (leftArray[index - 1].ToString() == "-")
                    {
                        sum -= Convert.ToDouble(leftArray[index].ToString());
                    }
                }
            }
            int trackerFlag = 0;
            foreach (int index in digitIndexArray)
            {
                leftArray.RemoveRange(index-1-trackerFlag, 2);

                trackerFlag= trackerFlag+2;
            }
            leftArray.Add("+");
            leftArray.Add(sum.ToString());

            return leftArray;
        }
        /*
         * 
         */
        static List<string> parserFunction(string equStr)
        {
            char[] disArray = equStr.ToArray();
            int i = 0;
            int nextFlag = 0;
            int nextSignFlag = 0;
            int xExistsFlag = 0;
            int bracketFlag = 0;
            int endbracketFlag = 0;
            int SignFlag = 0;
            string digit = "";
            string sign = "";
            List<int> tempSUM = new List<int>();
            List<int> tempADD = new List<int>();
            List<string> temp = new List<string>();
            foreach (char aChar in disArray)
            {
                if (aChar.ToString() == "1" || aChar.ToString() == "2" || aChar.ToString() == "3" || aChar.ToString() == "4" || aChar.ToString() == "5" || aChar.ToString() == "6" || aChar.ToString() == "7" || aChar.ToString() == "8" || aChar.ToString() == "9" || aChar.ToString() == "0" || aChar.ToString() == "X" || aChar.ToString() == "x")
                {
                    if (SignFlag == 1)
                    {
                        digit += aChar;
                        SignFlag = 0;
                    }
                    else if (SignFlag == 2)
                    {
                        digit = digit + "-" + aChar;
                        SignFlag = 0;
                    }
                    else { digit += aChar; }
                    if (aChar.ToString() == "X" || aChar.ToString() == "x") { xExistsFlag = 1; }
                    nextFlag = 1;
                    nextSignFlag = 0;
                }
                else if (aChar.ToString() == "+" || aChar.ToString() == "-" || aChar.ToString() == "%" || aChar.ToString() == "*" || aChar.ToString() == "/" || aChar.ToString() == "(" || aChar.ToString() == ")")
                {
                    if (nextFlag == 1){ temp.Add(digit); }
                    if ((digit.ToString() == "" || digit.ToString() == " ") && (aChar.ToString() == "%" || aChar.ToString() == "*" || aChar.ToString() == "/"))
                    {
                        if (endbracketFlag == 1)
                        {
                            temp.Add(aChar.ToString());
                            endbracketFlag = 0;
                        }
                        else
                        {
                            Console.WriteLine("Something wrong at Equation : Wrong format of sign");
                        }
                    }
                    else if (digit == "" && (aChar.ToString() == "+" || aChar.ToString() == "-"))
                    {
                        if (aChar.ToString() == "+") SignFlag = 1;
                        if (aChar.ToString() == "-") SignFlag = 2;
                        if (i>0 && i < disArray.Count())
                        {
                            if (disArray[i - 1].ToString() == ")" && disArray[i + 1].ToString() == "(") { temp.Add(aChar.ToString()); }
                        }
                        if (endbracketFlag == 1)
                        {
                            temp.Add(aChar.ToString());
                            endbracketFlag = 0;
                        } 

                    }
                    else
                    {
                        if (aChar.ToString() == "(" && disArray[i].ToString() == "(")
                        {
                            if (i==0)
                            {
                                
                            }
                            if (i!=0 && digitDetectFunction(disArray[i - 1].ToString()))
                            {
                                bracketFlag = 1;
                            }
                            if (i != 0 && disArray[i - 1].ToString() == ")")
                            {
                                bracketFlag = 1;
                            }
                        }
                        else if (aChar.ToString() == ")" && disArray[i].ToString() == ")")
                        {
                            if (disArray.Count() > i + 1)
                            {
                                if (digitDetectFunction(disArray[i + 1].ToString()))
                                {
                                    bracketFlag = 2;
                                }
                                if (disArray[i - 1].ToString() == "(")
                                {
                                    bracketFlag = 2;
                                }
                            }
                            endbracketFlag = 1;
                        }
                        
                        if (bracketFlag == 1) { temp.Add("*"); }
                        sign = aChar.ToString();
                        temp.Add(sign);
                        if (bracketFlag == 2) { temp.Add("*"); }
                        sign = "";
                        digit = "";
                        nextFlag = 0;
                        bracketFlag = 0;
                        //3 X + (12/ 4)3= - 2 - (2*4)+(2*2)

                    }
                    nextSignFlag = 1;
                }
                else if (aChar.ToString() == "^")
                {
                    if (digit.ToString() == "" || digit.ToString() == " ")
                    {
                        Console.WriteLine("Something wrong at right ^ ");
                    }
                    else
                    {
                        if (xExistsFlag == 1)
                        {
                            quadraticEquations = 1;
                            xExistsFlag = 0;
                        }
                        temp.Add(digit);
                        temp.Add(aChar.ToString());
                        digit = "";
                    }
                }

                i++;
            }

            if (digit != "") { temp.Add(digit); }
            else { Console.WriteLine("Something wrong at last right extra"); }
            return temp;
        }
        /*
         * 
         */
        static bool xdetectInListFunction(List<string> tempArray)
        {
            string pattern = @"[X]+";
            foreach (string temp in tempArray)
            {
                Match m = Regex.Match(temp, pattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    return true;
                }
            }
            return false;
        }
        /*
         * 
         */
        static bool xdetectFunction(string temp)
        {
            string pattern = @"[X]+";
           
            Match m = Regex.Match(temp, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return true;
            }
            return false;
        }
        /*
         * 
         */
        static bool digitDetectFunction(string temp)
        {
            int n;
            return int.TryParse(temp, out n);
        }
        /*
         * 
         */
        static string changeSign(string sign)
        {
            string chngeSign = "";
            if (sign == "*") { chngeSign = "/"; }
            else if (sign == "/") { chngeSign = "*"; } 
            else if (sign == "+") { chngeSign = "-"; }
            else if (sign == "-") { chngeSign = "+"; }
            return chngeSign;
        }
        /*
         * 
         */
        static void CheckDoubleAnswer(List<string> tempArray)
        {
            int xPerBracketCount = 0;
            int xDetectFlag = 0;
            int finishBracketFlag = 0;
            int startBracketFlag = 0;
            int totalIndex = tempArray.Count();
            int possibleDoubleAnswer = 0;
            int i = 0;
            foreach (string temp in tempArray)
            {
                if (temp == "(" && i - 2 < 0)
                {
                    if (tempArray[i - 1] == "*" && tempArray[i - 2] == ")")
                    {
                        possibleDoubleAnswer = 2;
                    }
                }
                else if (temp == "(")
                {
                    startBracketFlag = 1;
                }
                else if (temp == ")" && i + 2 < totalIndex)
                {
                    if (tempArray[i + 1] == "*" && tempArray[i + 1] == "(")
                    {
                        possibleDoubleAnswer = 1;
                    }
                    finishBracketFlag = 1;
                }
                else if (temp == ")")
                {
                    finishBracketFlag = 1;
                }
                else if (temp == "x" && temp == "X")
                {
                    if (startBracketFlag==1)
                    {
                        xPerBracketCount += 1;
                    }
                }
                i++;
            }
            
        }
        ///////////
    }
}
