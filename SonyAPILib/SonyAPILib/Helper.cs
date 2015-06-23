using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Management;
using System.Security.Cryptography;
using System.Diagnostics;


/// <summary>
/// Extra Extention Class
/// </summary>
public static class Extentions
{


    /// <summary>
    /// Chops Off all characters Before parameter
    /// </summary>
    /// <param name="s">The Full sting</param> 
    /// <param name="Before">The string to chop off to</param> 
    public static string ChopOffBefore(this string s, string Before)
    {//Usefull function for chopping up strings
        int End = s.ToUpper().IndexOf(Before.ToUpper());
        if (End > -1)
        {
            return s.Substring(End + Before.Length);
        }
        return s;
    }


    /// <summary>
    /// Chops Off all characters After parameter
    /// </summary>
    /// <param name="s">The Full sting</param> 
    /// <param name="After">The string to chop off after</param> 
    public static string ChopOffAfter(this string s, string After)
    {//Usefull function for chopping up strings
        int End = s.ToUpper().IndexOf(After.ToUpper());
        if (End > -1)
        {
            return s.Substring(0, End);
        }
        return s;
    }


    /// <summary>
    /// Replaces string contents while ignoring case
    /// </summary>
    /// <param name="Source">The Full sting</param> 
    /// <param name="Pattern">The string to remove</param> 
    /// <param name="Replacement">The string to replace</param>
    /// <returns>Returns replaced string</returns>
    public static string ReplaceIgnoreCase(this string Source, string Pattern, string Replacement)
    {// using \\$ in the pattern will screw this regex up
        //return Regex.Replace(Source, Pattern, Replacement, RegexOptions.IgnoreCase);

        if (Regex.IsMatch(Source, Pattern, RegexOptions.IgnoreCase))
            Source = Regex.Replace(Source, Pattern, Replacement, RegexOptions.IgnoreCase);
        return Source;
    }

}


