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

        static void Main(string[] args)
        {
            /*
             3 X + 12/ 4= 3^2  +2*2    - 2 - 2*2
             3 X + 12/ 4= (8/2*2+4)  +2*2    - 2 - 2*4
             3 X + 12/ 4= (8/2*2+4X)  +2*2    - 2 - 2*4
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
                    Console.WriteLine("Right-------------------------");
                    rightArray = parserFunction(rightStr);
                    Console.WriteLine("result------------------------");
                    calcFunction(leftArray, rightArray);
                }
                else
                {
                    Console.WriteLine("Something wrong at = ");

                }
            }
           
        }
        static void calcFunction(List<string> leftArray, List<string> rightArray)
        {
            listcalcFunction(rightArray,"right");
            listcalcFunction(leftArray, "left");
        }
        static string listcalcFunction(List<string> tempArray,string side)
        {
            int stopFlag = 1;
            while (stopFlag != 0)
            {
                if (tempArray.Contains<string>("(") && tempArray.Contains<string>(")"))
                {
                    int index = 0;
                    int startindex = tempArray.IndexOf("(");
                    int endindex = tempArray.IndexOf(")");
                    int indexGap = endindex - startindex;
                    List<string> total = new List<string>();
                    List<string> tempList = new List<string>();
                    for (int i = startindex + 1; i < endindex; i++)
                    {
                        tempList.Add(tempArray[i].ToString());
                    }
                    if (xdetectInListFunction(tempArray))
                    {
                        total = sublistcalcFunction(tempList, 1, side);
                    }
                    else
                    {
                        total = sublistcalcFunction(tempList, 0, side);
                    }
                    Console.WriteLine("(.................................)");
                    if (total.Count == 3)
                    {
                        tempArray.Insert(startindex, total[0].ToString());
                        tempArray.Insert(startindex, total[1].ToString());
                        tempArray.Insert(startindex, total[2].ToString());
                        tempArray.RemoveRange(startindex + 3, indexGap + 1);
                    }
                    else if (total.Count == 1)
                    {
                        tempArray.Insert(startindex, total[0].ToString());
                        tempArray.RemoveRange(startindex + 1, indexGap + 1);
                    }

                }
                else if (side == "right" && xdetectInListFunction(tempArray))
                {
                    int index = 0;
                    int flag = 0;
                    string value = "";
                    List<string> total = new List<string>();
                    total = sublistcalcFunction(tempArray, 1, side);
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
                        leftArray.Add("-");
                        leftArray.Add(value);
                        if (total[index + 1].ToString() == "+")
                        {
                            total.RemoveRange(index, 2);
                        }
                        else if (total[index + 1].ToString() == "-")
                        {
                            string valueString = total[index + 2].ToString();
                            int valueNumber = Convert.ToInt32(valueString);
                            valueNumber = valueNumber * (-1);
                            total.RemoveRange(index, 2);
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
                    total = sublistcalcFunction(tempArray, 1, side);
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
                            }
                            else if (xIndex > 0)
                            {
                                if (total[xIndex-1].ToString()=="+")
                                {
                                    string temp = total[xIndex].ToString();
                                    temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                    if (temp != "") { sum = sum + Convert.ToInt32(temp); }
                                }
                                else if (total[xIndex - 1].ToString() == "-")
                                {
                                    string temp = total[xIndex].ToString();
                                    temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                    if (temp != "") { sum = sum - Convert.ToInt32(temp); }
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
                                    rightSideResult = rightSideResult- Convert.ToInt32(temp);
                                }
                                else if (total[digitIndex - 1].ToString() == "-")
                                {
                                    string temp = total[digitIndex].ToString();
                                    rightSideResult = rightSideResult + Convert.ToInt32(temp);
                                }
                            }
                            rightSideResult = rightSideResult / sum;
                        }
                    }
                    Console.WriteLine("result of x ="+ rightSideResult);
                    break;
                }
                else
                {
                    sublistcalcFunction(tempArray, 0, side);
                }
                if (tempArray.Count()==1)
                {
                    stopFlag = 0;
                }
            }
            return tempArray[0].ToString();
        }

        static List<string> sublistcalcFunction(List<string> tempArray,int xExists,string side){
            int stopFlag = 1;
            int priorityFlag = 0;
            List<string> priorityArray = new List<string>();
            List<string> priorityTempArray = new List<string>();
            foreach (string aChar in tempArray)
            {
                    if (aChar == "^") { priorityArray.Add("^"); }
                    else if (aChar == "*") { priorityArray.Add("*"); }
                    else if (aChar == "/") { priorityArray.Add("/"); }
                    else if (aChar == "%") { priorityArray.Add("%"); }
                    else if (aChar == "+") { priorityTempArray.Add("+"); }
                    else if (aChar == "-") { priorityTempArray.Add("-"); }
            }
            priorityArray.AddRange(priorityTempArray);
            foreach (string aChar in priorityArray)
            {
                if (aChar.ToString() == "+" || aChar.ToString() == "-" || aChar.ToString() == "%" || aChar.ToString() == "*" || aChar.ToString() == "/" || aChar.ToString() == "^")
                {
                    if (aChar == "^") { priorityFlag = 1; }
                    else if (aChar == "*") { priorityFlag = 2; }
                    else if (aChar == "/") { priorityFlag = 3; }
                    else if (aChar == "%") { priorityFlag = 4; }
                    else if (aChar == "+") { priorityFlag = 5; }
                    else if (aChar == "-") { priorityFlag = 6; }
                    stopFlag = 1;
                    ///asdasd
                }
                if (tempArray.Contains<string>("^") && priorityFlag==1)
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
                if (tempArray.Contains<string>("*") && priorityFlag == 2)
                {
                    priorityFlag = 0;
                    int index = 0;
                    int tempTotal = 0;
                    index = tempArray.IndexOf("*");
                    string tempLeft = tempArray[index - 1].ToString();
                    string tempRight = tempArray[index + 1].ToString();
                    if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                    {
                        tempTotal = Convert.ToInt32(tempLeft) * Convert.ToInt32(tempRight);
                        Console.WriteLine("step--" + tempLeft + "*" + tempRight + "----" + tempTotal);
                        tempArray.Insert(index - 1, tempTotal.ToString());
                        tempArray.RemoveRange(index, 3);
                    }

                }
                if (tempArray.Contains<string>("/") && priorityFlag == 3)
                {
                    priorityFlag = 0;
                    int index = 0;
                    int tempTotal = 0;
                    index = tempArray.IndexOf("/");
                    string tempLeft = tempArray[index - 1].ToString();
                    string tempRight = tempArray[index + 1].ToString();
                    if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                    {
                        tempTotal = Convert.ToInt32(tempLeft) / Convert.ToInt32(tempRight);
                        Console.WriteLine("step--" + tempLeft + "/" + tempRight + "----" + tempTotal);
                        tempArray.Insert(index - 1, tempTotal.ToString());
                        tempArray.RemoveRange(index, 3);
                    }

                }
                if (tempArray.Contains<string>("+") && priorityFlag == 5)
                {
                    priorityFlag = 0;
                    int index = 0;
                    int tempTotal = 0;
                    index = tempArray.IndexOf("+");
                    string tempLeft = tempArray[index - 1].ToString();
                    string tempRight = tempArray[index + 1].ToString();
                    if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                    {
                        tempTotal = Convert.ToInt32(tempLeft) + Convert.ToInt32(tempRight);
                        Console.WriteLine("step--" + tempLeft + "+" + tempRight + "----" + tempTotal);
                        tempArray.Insert(index - 1, tempTotal.ToString());
                        tempArray.RemoveRange(index, 3);
                    }
                }
                if (tempArray.Contains<string>("-") && priorityFlag == 6)
                {
                    priorityFlag = 0;
                    int index = 0;
                    int tempTotal = 0;
                    index = tempArray.IndexOf("-");
                    string tempLeft = tempArray[index - 1].ToString();
                    string tempRight = tempArray[index + 1].ToString();
                    if (!xdetectFunction(tempLeft) && !xdetectFunction(tempRight))
                    {
                        tempTotal = Convert.ToInt32(tempLeft) - Convert.ToInt32(tempRight);
                        Console.WriteLine("step--" + tempLeft + "-" + tempRight + "----" + tempTotal);
                        tempArray.Insert(index - 1, tempTotal.ToString());
                        tempArray.RemoveRange(index, 3);
                    }
                }
                
            }
            return tempArray;
        }

        static List<string> parserFunction(string equStr)
        {
            char[] disArray = equStr.ToArray();
            int i = 0;
            int nextFlag = 0;
            int spaceFlag = 0;
            int powerFlag = 0;
            string digit = "";
            string sign = "";
            string tempdigit = "";
            List<int> tempSUM = new List<int>();
            List<int> tempADD = new List<int>();
            List<string> temp = new List<string>();

            foreach (char aChar in disArray)
            {
                if (aChar.ToString() == "1" || aChar.ToString() == "2" || aChar.ToString() == "3" || aChar.ToString() == "4" || aChar.ToString() == "5" || aChar.ToString() == "6" || aChar.ToString() == "7" || aChar.ToString() == "8" || aChar.ToString() == "9" || aChar.ToString() == "0" || aChar.ToString() == "X" || aChar.ToString() == "x")
                {
                    digit += aChar;
                    nextFlag = 1;
                }
                else if (aChar.ToString() == "+" || aChar.ToString() == "-" || aChar.ToString() == "%" || aChar.ToString() == "*" || aChar.ToString() == "/" || aChar.ToString() == "(" || aChar.ToString() == ")")
                {
                    if (nextFlag == 1)
                    {
                        temp.Add(digit);
                    }
                    if ((digit.ToString() == "" || digit.ToString() == " ") && (aChar.ToString() == "%" || aChar.ToString() == "*" || aChar.ToString() == "/" ))
                    {
                        Console.WriteLine("Something wrong at right + ");
                    } 
                    else if (digit == "" && (aChar.ToString() == "+" || aChar.ToString() == "-" || aChar.ToString() == "(" || aChar.ToString() == ")"))
                    {
                        sign = aChar.ToString();
                        temp.Add(sign);
                        sign = "";
                    }
                    else
                    {
                        sign = aChar.ToString();
                        temp.Add(sign);
                        sign = "";
                        digit = "";
                        nextFlag = 0;
                    }

                }
                else if (aChar.ToString() == "^")
                {
                    if (digit.ToString() == "" || digit.ToString() == " ")
                    {
                        Console.WriteLine("Something wrong at right ^ ");
                    }
                    else
                    {
                        temp.Add(digit);
                        temp.Add(aChar.ToString());
                        digit = "";
                    }
                }
            }

            if (digit != "")
            {
                temp.Add(digit);
            }
            else
            {
                Console.WriteLine("Something wrong at last right extra");
            }

            return temp;
        }
        static bool xdetectInListFunction(List<string> tempArray)
        {
            int flag = 0;
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
        static bool xdetectFunction(string temp)
        {
            int flag = 0;
            string pattern = @"[X]+";
           
            Match m = Regex.Match(temp, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return true;
            }
            return false;
        }
        static bool digitDetectFunction(string temp)
        {
            int n;
            bool isNumeric = int.TryParse(temp, out n);
            return isNumeric;
        }
        static string changeSign(string sign)
        {
            string chngeSign = "";
            if (sign == "*") { chngeSign = "/"; }
            else if (sign == "/") { chngeSign = "*"; } 
            else if (sign == "+") { chngeSign = "-"; }
            else if (sign == "-") { chngeSign = "+"; }
            return chngeSign;
        }

    }
}
