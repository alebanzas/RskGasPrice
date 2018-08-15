using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace GasPrice.Data
{
	/// <summary>
	/// This class opens a website and scraps the contents
	/// </summary>
	public class Scraper
    {
        private readonly Encoding _encoding;
        private readonly string _referer;

        public HttpWebRequest Request { get; set; }

        public Scraper(Uri url)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);
	    }

        public Scraper(Uri url, Encoding encoding)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);
            _encoding = encoding;
        }

        public Scraper(Uri url, Encoding encoding, string referer)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);
            _encoding = encoding;
            _referer = referer;
        }

	    public HtmlNode GetNodes()
	    {
	        return GetNodes(HttpMethod.Get, null);
	    }

        public HtmlNode GetNodes(HttpMethod method, Dictionary<string, object> postParameters)
        {
            // Create the WebRequest for the URL we are using
            Request.Method = method.Method;

            if (HttpMethod.Post.Equals(method))
            {
                if (postParameters != null)
                {
                    var postData = new StringBuilder();
                    foreach (var postParameter in postParameters)
                    {
                        postData.Append($"{postParameter.Key}={HttpUtility.UrlEncode(postParameter.Value.ToString())}&");
                    }

                    byte[] postBytes = _encoding.GetBytes(postData.ToString());

                    Request.ContentType = "application/x-www-form-urlencoded";
                    Request.ContentLength = postBytes.Length;

                    var postStream = Request.GetRequestStream();
                    postStream.Write(postBytes, 0, postBytes.Length);
                    postStream.Flush();
                    postStream.Close();
                }
            }

            if (!string.IsNullOrWhiteSpace(_referer))
            {
                Request.Referer = _referer;
            }

            // Get the stream from the returned web response
            var stream = _encoding != null ? new StreamReader(
                Request.GetResponse().GetResponseStream() ?? 
                throw new InvalidOperationException(), _encoding) : 
                new StreamReader(
                    Request.GetResponse().GetResponseStream() ?? 
                    throw new InvalidOperationException());

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(stream);
            return htmlDocument.DocumentNode;
        }
	}
}
