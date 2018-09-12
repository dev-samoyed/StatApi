using StatApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace StatApi.Controllers
{
    public class CrimesController : ApiController
    {
        Context db = new Context();
        // 5.06.00.01 Число зарегистрированных преступлений ..xls
        // GET: api/crime
        public List<Crime> GetAll()
        {
            return db.Crimes.ToList();
        }

        //// GET: api/crime/{id}
        [Route("api/Crimes/{request}/{id}")]
        public Crime Get(Guid? id, string request)
        {
            //if(request!=null)
            //    GetFile(request);

            Crime crime = db.Crimes.Where(x => x.Id == id).FirstOrDefault();

            return crime;
        }
        [HttpGet]
        [Route("api/Crimes/{request}")]
        public IHttpActionResult GetFile(string request)
        {
            var crime = db.Crimes.Where(x => x.Request == request.ToLower()).FirstOrDefault();
            string filePath = HttpRuntime.AppDomainAppPath + $"\\Files\\Crimes\\{crime.PathToFile}";
            var id = Guid.NewGuid();
            string xlsxName = $"{id}.xls";
            
            var dataBytes = File.ReadAllBytes(filePath);
            var dataStream = new MemoryStream(dataBytes);
            return new xlsxResult(dataStream, Request, xlsxName);
        }

        
        // POST: api/crime
        public void Post([FromBody]Crime crime)
        {
            crime.Id = Guid.NewGuid();
            db.Crimes.Add(crime);
            db.SaveChanges();
        }
        
        // DELETE: api/crime/{id}
        public void Delete(Guid? id)
        {
            var crime = db.Crimes.Where(z => z.Id == id).FirstOrDefault();
            db.Crimes.Remove(crime);
        }

        
    }

    public class xlsxResult : IHttpActionResult
    {
        MemoryStream fileStuff;
        string XlsxFileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public xlsxResult(MemoryStream data, HttpRequestMessage request, string filename)
        {
            fileStuff = data;
            httpRequestMessage = request;
            XlsxFileName = filename;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(fileStuff);
            //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = XlsxFileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }
    }
}
