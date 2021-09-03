using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LeetCode
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      var s = new Solution();
      //var d = s.LenGroups("pipppo123333443333", out args, ou );

      //foreach (var k in d.Keys)
      //{
      //  Console.WriteLine($"Seq da {k} sono {d[k]}");
      //}

      Console.WriteLine(s.StrongPasswordChecker("..."));

    }
  }
  


  public class Solution
  {
    private const int MAX_LEN = 20;
    private const int MIN_LEN = 6;

    public Dictionary<int,int> LenGroups(string password, out bool existsLowerCase, out bool existsUpperCase, out bool existsDigit)
    {
      var l = new Dictionary<int, int>();
      char? currChar = null;
      int currSeq = 0;
      existsUpperCase = false;
      existsLowerCase = false;
      existsDigit = false;
      foreach (char c in password)
      {
        if (Char.IsDigit(c))
          existsDigit = true;
        else if (Char.IsLower(c))
          existsLowerCase = true;
        else if (Char.IsUpper(c))
          existsUpperCase = true;

        if (currChar == null)
        {
          currSeq = 1;
          currChar = c;
        }
        else if (currChar == c)
          currSeq++;
        else
        {
          if (currSeq > 2)
          {
            if (!l.TryAdd(currSeq,1))
              l[currSeq] = l[currSeq]+1;
          }
          currSeq = 1;
          currChar = c;
        }
      }

      if (currSeq > 2)
      {
        if (!l.ContainsKey(currSeq))
          l.Add(currSeq, 1);
        else
          l[currSeq] = l[currSeq] + 1;

      }
      return l;
    }


    public int StrongPasswordChecker(string password)
    {
      bool existsLowerCase = false;
      bool existsUpperCase = false;
      bool existsDigit = false;

      int numStepReplace;
      int numStepDelete = 0;
      int numStepInsert = 0;

      var gruppi = LenGroups(password, out existsLowerCase, out existsUpperCase,out  existsDigit);

      var pwdLen = password.Length;

      if (pwdLen < 3)
        return MIN_LEN - pwdLen;

      while (pwdLen > MAX_LEN)
      {
        numStepDelete++;
        if (gruppi.Any())
        {
          var bestKey = gruppi.Keys.OrderBy(x => x%3).FirstOrDefault();
          if (bestKey == 0)
            bestKey = gruppi.Keys.First();

          gruppi[bestKey] = gruppi[bestKey] - 1;
          if (gruppi[bestKey] == 0)
            gruppi.Remove(bestKey);
          if (bestKey > 3)
          {
            if (gruppi.ContainsKey(bestKey - 1))
              gruppi[bestKey - 1] = gruppi[bestKey - 1] + 1;
            else
              gruppi.Add(bestKey - 1, 1);
          }
        }

        pwdLen--;
      }

      if (pwdLen < MIN_LEN)
      {
        if (pwdLen == 5)
        {
          numStepInsert++;
          var maxKey = gruppi.Keys.Any() ? gruppi.Keys.First() : 0;
          if (maxKey == 5)
          {
            gruppi.Remove(5);
            gruppi.Add(3, 1);
          }
          else
            gruppi.Clear();
          pwdLen++;
        }
        gruppi.Clear();

        numStepInsert += MIN_LEN - pwdLen;
        //pwdLen = MIN_LEN;
      }


      //if (pwdLen < MIN_LEN)
      //  gruppi.Clear();

      //while (pwdLen < MIN_LEN)
      //{
      //  numStepInsert++;
      //  pwdLen++;
      //}


      var numReplPerGruppi = 0;
      foreach (var k in gruppi.Keys)
        numReplPerGruppi += (k / 3)*gruppi[k];

      var needs = 0;
      if (!existsDigit) needs++;
      if (!existsLowerCase) needs++;
      if (!existsUpperCase) needs++;

      numStepReplace = numReplPerGruppi;
      if (needs > numReplPerGruppi + numStepInsert)
        numStepReplace += needs - (numReplPerGruppi + numStepInsert);


      return numStepInsert + numStepReplace + numStepDelete;
    }
  }
}
