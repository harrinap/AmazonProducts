/**********************************************************************************************
 * Copyright 2009 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file 
 * except in compliance with the License. A copy of the License is located at
 *
 *       http://aws.amazon.com/apache2.0/
 *
 * or in the "LICENSE.txt" file accompanying this file. This file is distributed on an "AS IS"
 * BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under the License. 
 *
 * ********************************************************************************************
 *
 *  Amazon Product Advertising API
 *  Signed Requests Sample Code
 *
 *  API Version: 2009-03-31
 *
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Collections;
using System.Data.SQLite;

namespace AmazonProductAdvtApi 
{
    class ItemLookupSample
    {
        private const string MY_AWS_ACCESS_KEY_ID = "AKIAIYL2J2DJQG3XMHIQ";
        private const string MY_AWS_SECRET_KEY = "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";
        private const string DESTINATION          = "ecs.amazonaws.com";
        
        private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/2009-03-31";
        private const string ITEM_ID   = "0545010225";

        public static void Main()
        {

            InitDB();
            SignedRequestHelper helper = new SignedRequestHelper(MY_AWS_ACCESS_KEY_ID, MY_AWS_SECRET_KEY, DESTINATION);
 
            String requestUrl;

            
 
            System.Console.WriteLine("Method 2: Query String form.");

            String[] Keywords = new String[] {
                "surprise!",
                "café",
                "black~berry",
                "James (Jim) Collins",
                "münchen",
                "harry potter (paperback)",
                "black*berry",
                "finger lickin' good",
                "!\"#$%'()*+,-./:;<=>?@[\\]^_`{|}~",
                "αβγδε",
                "ٵٶٷٸٹٺ",
                "チャーハン",
                "叉焼",
            };
            var keyword = "men's shoes size 8";

            //foreach (String keyword in Keywords)
            //{
            String requestString = "Service=AWSECommerceService"
                + "&Version=2009-03-31"
                + "&Operation=ItemLookup"
                + "&SearchIndex=All"
                + "&ResponseGroup=VariationMatrix"
                + "&AssociateTag=aztag-20"
                + "&ItemId=B00AEVFNSY"
                //+ "&Keywords=" + keyword;
                    ;
                requestUrl = helper.Sign(requestString);
                string[] topSellerTitles = FetchTitles(requestUrl);

                foreach (string s in topSellerTitles)
                {
                    System.Console.WriteLine(s);
                    Console.ReadLine();
                }
                
                System.Console.WriteLine();
          

            System.Console.WriteLine("Hit Enter to end");
            System.Console.ReadLine();
        }

        private static void InitDB()
        {

            DataStorage.InitializeSearchIndexes();

            
        }

        private static string FetchTitle(string url)
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message", NAMESPACE);
                if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                {
                    String message = errorMessageNodes.Item(0).InnerText;
                    return "Error: " + message + " (but signature worked)";
                }
               
          
                XmlNode titleNode = doc.GetElementsByTagName("Title").Item(0);
                string title = titleNode.InnerText;
                return title;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Caught Exception: " + e.Message);
                System.Console.WriteLine("Stack Trace: " + e.StackTrace);
            }

            return null;
        }

        private static string[] FetchTitles(string url)
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                //XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message", NAMESPACE);
                //if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                //{
                //    String message = errorMessageNodes.Item(0).InnerText;
                //    return "Error: " + message + " (but signature worked)";
                //}


               XmlNodeList  titleNodes = doc.GetElementsByTagName("Title");
               List<string> titles = new  List<String>();
               foreach (XmlNode titleNode in titleNodes)
               {
                   titles.Add(titleNode.InnerText);
               }
               return titles.ToArray();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Caught Exception: " + e.Message);
                System.Console.WriteLine("Stack Trace: " + e.StackTrace);
            }

            return null;

        }
    }
}
