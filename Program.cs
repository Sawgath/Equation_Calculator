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
        /*
         * Start of the code
         * 
         * method naming - Camel case ( camelCase)
         * variable naming - Camel case ( camelCase)
         * 
         * quadraticEquations indicate inputed equation is a quadratic Equations.
         * LeftArray contains left side of the equation
         * RightArray contains left side of the equation
         * 
         * temp keyword indicate temporary variable or list  for operation purpose.
         * Flag keyword is use for deciding or changing flow of the process or decision making.
         * 
         */
        public static List<string> leftArray { get;  set; }
        public static List<string> rightArray { get;  set; }
        public static int quadraticEquations = 0;
        /*
         * Main function
         */
        static void Main(string[] args)
        {
            leftArray=new List<string>();
            rightArray = new List<string>();
            string readStr = "";
            string leftStr = "";
            string rightStr = "";
            /*
            * Feeding input Until exit is inputed.
            */
            while (readStr != "exit")
            {
                Console.WriteLine("");
                Console.WriteLine("PS: If you want to close the program Type -> exit ");
                Console.WriteLine("Enter your equation-");
                readStr = Console.ReadLine();
                readStr=readStr.Replace(" ",string.Empty);
                Console.WriteLine("After removing space-"+readStr);
                /*
                * Speartaing the left side and right side.
                */
                List<string> divideLeftRight = readStr.Split('=').ToList();
                if (divideLeftRight.Count == 2)
                {
                    leftStr=divideLeftRight.ElementAt<string>(0);
                    rightStr = divideLeftRight.ElementAt<string>(1);
                    /*
                    * Parsing each value, sign and add them into string list.
                    */
                    leftArray = parserFunction(leftStr);
                    rightArray = parserFunction(rightStr);
                    /*
                    * Decision making function for the equation.
                    */
                    equationDeciderFunction(leftArray, rightArray);
                }
                else
                {
                    Console.WriteLine("Error- Missing syntex (=) OR Invalid syntex near (=)");
                }
            }
        }
        /*
         * This function call the other function based on the equation.
         * If the equation is a quadratic Equation, it will call the  quadratic Equation related function.
         * Check the x from the equation 
         */
        static void equationDeciderFunction(List<string> leftArray, List<string> rightArray)
        {
            int noXValueFlag = 0;
            if (xdetectInListFunction(leftArray)==false)
            {
                noXValueFlag += 1;
            }
            if (xdetectInListFunction(rightArray) == false)
            {
                noXValueFlag += 1;
            }
            if (quadraticEquations == 1 && noXValueFlag != 2)
            {
                quadraticEquationsFunction(leftArray, rightArray);
            }
            else if (noXValueFlag != 2)
            {
                decisionMakingFunction(rightArray, "right");
                decisionMakingFunction(leftArray, "left");
            }
            else
            {
                Console.WriteLine("Error- Missing syntex (x)");
            }
        }
        /*
         * This function find take all the decision on equation base on single layer parenthesis,Left site calculation decision,right site calculation decision,
         * quadratic Equations calculation decision and conditions regarding operators, negative numbers.
         */
        static void decisionMakingFunction(List<string> tempArray,string side)
        {
            try
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
                        if (startindex > 1 && tempArray[startindex - 1].ToString() == "*")
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
                        if (multiplyVariable != "")
                        {
                            multiplyVariableFlag = 1;
                            List<string> temptotal = new List<string>();
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
                                        temptotal.Add(multiplyTotal + "x");
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

                        if (total.Count == 3)
                        {
                            if (startindex != 0 && digitDetectFunction(tempArray[startindex - 1].ToString()))
                            {
                            }
                            else if (multiplyVariableFlag == 1)
                            {
                                tempArray.Insert(startindex - 2, total[2].ToString());
                                tempArray.Insert(startindex - 2, total[1].ToString());
                                tempArray.Insert(startindex - 2, total[0].ToString());
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
                            tempArray.Insert(startindex - 2, total[0].ToString());
                            tempArray.RemoveRange(startindex - 1, indexGap + 3);
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
                        double rightSideResult = 0;
                        double sum = 0;
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
                                    if (temp != "") { sum = sum + Convert.ToDouble(temp); }
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
                                            if (temp != "") { sum = sum / Convert.ToDouble(temp); }
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
                                            sum = sum + Convert.ToDouble(temp1) / Convert.ToDouble(temp2);
                                        }

                                    }
                                    else if (total[xIndex - 1].ToString() == "+")
                                    {
                                        string temp = total[xIndex].ToString();
                                        temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                        if (temp != "") { sum = sum + Convert.ToDouble(temp); }
                                        else { sum = sum + 1; }
                                    }
                                    else if (total[xIndex - 1].ToString() == "-")
                                    {
                                        string temp = total[xIndex].ToString();
                                        temp = new string(temp.Where(c => Char.IsDigit(c)).ToArray());
                                        if (temp != "") { sum = sum - Convert.ToDouble(temp); }
                                        else { sum = sum - 1; }
                                    }
                                }
                            }

                            rightSideResult = Convert.ToInt32(rightArray[0].ToString());
                            foreach (int digitIndex in digitIndexArray)
                            {
                                if (digitIndex == 0)
                                {
                                    string temp = total[digitIndex].ToString();
                                    rightSideResult = rightSideResult - Convert.ToDouble(temp);
                                }
                                else if (digitIndex > 0)
                                {
                                    if (total[digitIndex - 1].ToString() == "+")
                                    {
                                        string temp = total[digitIndex].ToString();
                                        rightSideResult = rightSideResult - Convert.ToDouble(temp);
                                    }
                                    else if (total[digitIndex - 1].ToString() == "-")
                                    {
                                        string temp = total[digitIndex].ToString();
                                        rightSideResult = rightSideResult + Convert.ToDouble(temp);
                                    }
                                    else if (total[digitIndex - 1].ToString() == "/")
                                    {
                                        string temp = total[digitIndex].ToString();
                                        rightSideResult = rightSideResult * Convert.ToDouble(temp);
                                    }
                                    else if (total[digitIndex - 1].ToString() == "*")
                                    {
                                        string temp = total[digitIndex].ToString();
                                        rightSideResult = rightSideResult / Convert.ToDouble(temp);
                                    }
                                }
                            }
                            if (sum != 0)
                            {
                                rightSideResult = rightSideResult / sum;
                            }
                        }
                        Console.WriteLine("Result of x =" + rightSideResult);
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
                        if (quardicXindexArray.Count > 0 && quardicXflag == 1 && side == "qleft")
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
                                leftArray = new List<string>();
                                rightArray = new List<string>();
                                tempArray = new List<string>();
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
                                leftArray = new List<string>();
                                rightArray = new List<string>();
                                tempArray = new List<string>();
                            }
                        }
                        Console.WriteLine("Result of x =" + positiveResult + " or " + negativeResult);
                        stopFlag = 0;
                    }
                    else
                    {
                        calculationFunction(tempArray, 0, side);
                    }

                    if (tempArray.Count() == 1)
                    {
                        stopFlag = 0;
                    }
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Error- Infinite value generated .");
                tempArray = new List<string>();
                leftArray = new List<string>();
                rightArray = new List<string>();
            }
            catch (DivideByZeroException eDivideByZeroException)
            {
                Console.WriteLine("Error- Divided by zero .");
                tempArray = new List<string>();
                leftArray = new List<string>();
                rightArray = new List<string>();
            }
            catch (IndexOutOfRangeException eIndexOutOfRangeException)
            {
                Console.WriteLine("Error- Out of Integer Range errors.");
                tempArray = new List<string>();
                leftArray = new List<string>();
                rightArray = new List<string>();
            }
        }
        /*
         * This function performs all the calculation based on the operators.
         * xExists variable in the parameter indicate the provided list of string contain X or not.
         * side variable in the parameter indicate left side or right side of the equation.
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
                            //Console.WriteLine("step--" + tempLeft + "^" + tempRight + "----" + tempTotal);
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
                            //Console.WriteLine("step--" + tempLeft + "^" + tempRight + "----" + tempTotal);
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
                            //Console.WriteLine("step--" + tempLeft + "*" + tempRight + "----" + tempTotal);
                            tempArray.Insert(index - 1, tempTotal.ToString());
                            tempArray.RemoveRange(index, 3);
                            escapeFlag = 1;
                        }
                        if (escapeFlag ==0)
                        {
                            if (index > 0 && digitDetectFunction(tempArray[index - 1].ToString()) && tempArray[index + 1].ToString() == "X" || tempArray[index + 1].ToString() == "x")
                            {
                                string tempString = tempArray[index - 1].ToString() + tempArray[index + 1].ToString();
                                //Console.WriteLine("step--" + tempString + "*" + tempString + "----" + tempString);
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
                            //Console.WriteLine("step--" + tempLeft + "/" + tempRight + "----" + tempTotal);
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
                            //Console.WriteLine("step--" + tempLeft + "+" + tempRight + "----" + tempTotal);
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

                                //Console.WriteLine("step--" + tempLeft + "-" + tempRight + "----" + tempTotal);
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
                Console.WriteLine("Error- Divided by zero .");
                tempArray = new List<string>();
                leftArray = new List<string>();
                rightArray = new List<string>();
                return tempArray;
            }
            catch (IndexOutOfRangeException eIndexOutOfRangeException)
            {
                Console.WriteLine("Error- Out of Integer Range errors.");
                tempArray = new List<string>();
                leftArray = new List<string>();
                rightArray= new List<string>();
                return tempArray;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Error- Infinite value generated .");
                tempArray = new List<string>();
                leftArray = new List<string>();
                rightArray = new List<string>();
                return tempArray;
            }
        }
        /*
         *  For quadratic equation, quadraticEquationsSmiplificationFunction,decisionMakingFunction is called from this function.
         *  First it simplfy the equation.(quadraticEquationsSmiplificationFunction)
         *  Then pass it to decisionMakingFunction.
         *  Then calculate the equation through calculationFunction.
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
         * This function responsible for complex quadratic equation in to ax^2+bx+c format.
         * This function peform all the operation on digit number based on the operator.Neglect x related opertaion.
         * This function return list of string.
         */
        static List<string> quadraticEquationsSmiplificationFunction(List<string> leftArray)
        {
            int i = 0;
            List<int> digitIndexArray = new List<int>();
            List<string> priorityArray = new List<string>();
            try
            { 
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
                            //Console.WriteLine("step--" + tempLeft + "^" + tempRight + "----" + tempTotal);
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
                            //Console.WriteLine("step--" + tempLeft + "*" + tempRight + "----" + tempTotal);
                            leftArray.Insert(index - 1, tempTotal.ToString());
                            leftArray.RemoveRange(index, 3);
                            escapeFlag = 1;
                        }
                        if (escapeFlag == 0)
                        {
                            if (index > 0 && digitDetectFunction(leftArray[index - 1].ToString()) && leftArray[index + 1].ToString() == "X" || leftArray[index + 1].ToString() == "x")
                            {
                                string tempString = leftArray[index - 1].ToString() + leftArray[index + 1].ToString();
                                //Console.WriteLine("step--" + tempString + "*" + tempString + "----" + tempString);
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
                            //Console.WriteLine("step--" + tempLeft + "/" + tempRight + "----" + tempTotal);
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
            catch (DivideByZeroException eDivideByZeroException)
            {
                Console.WriteLine("Error- Divided by zero .");
                
                leftArray = new List<string>();
                rightArray = new List<string>();
                return leftArray;
            }
            catch (IndexOutOfRangeException eIndexOutOfRangeException)
            {
                Console.WriteLine("Error- Out of Integer Range errors.");
                
                leftArray = new List<string>();
                rightArray = new List<string>();
                return leftArray;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Error- Infinite value generated .");
                leftArray = new List<string>();
                rightArray = new List<string>();
                return leftArray;
            }
        }
        /*
         * This function responsible for parsing the string into digit, sign and x values.
         * At first it check all the digit, sign and x values from the string Then add them separately into a list.
         * This function return list of string.
         */
        static List<string> parserFunction(string equationString)
        {
            char[] disArray = equationString.ToArray();
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
                            Console.WriteLine("Error - Wrong format of sign");
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
                        Console.WriteLine("Error - Wrong syntex near ^ ");
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
            else if(endbracketFlag==1 && digit == "")
            {
            }
            else { Console.WriteLine("Warning - Wrong syntex near last value at left side or right side of the equation"); }
            int listCount = temp.Count();
            /*
             * Resolving blank number symbols with 0.
             */
            if (temp[listCount - 1].ToString() == "+" || temp[listCount - 1].ToString() == "-")
            {
                temp.Add("0");
                string viewString = "";
                foreach(string view in temp)
                {
                   viewString = viewString + view;
                }
                Console.WriteLine("Resolved part of the equation - "+ viewString);
            }

            return temp;
        }
        /*
         * This function responsible for finding (X or x) in a List.
         * This function return true if x is found in the List, otherwise returns false. 
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
         * This function responsible for finding (X or x) in a string.
         * This function return true if x is found in the string, otherwise returns false.
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
         * This function responsible for checking digit in a string.
         * This function return true if the whole string is digit, otherwise returns false.
         */
        static bool digitDetectFunction(string temp)
        {
            int n;
            return int.TryParse(temp, out n);
        }
        /*
         * This function responsible for changing opposite sign.Like (+ -> -),(- -> *),(* -> /),(/ -> *)
         * This funtion mostly use in changing site.Like Moving  then number -2 from left site to right site.
         * This function return sign that should be change as per the condition otherwise it returns empty space.
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
         * End of the code
         */
    }
}
