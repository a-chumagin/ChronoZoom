using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using Application.Driver;
using Application.Helper.Constants;
using Application.Helper.Entities;
using Application.Helper.UserActions;
using System.IO;
using System.Runtime.Serialization.Json;
using Chronozoom.Entities;
using ContentItem = Chronozoom.Entities.ContentItem;
using Exhibit = Application.Helper.Entities.Exhibit;
using Timeline = Application.Helper.Entities.Timeline;
using Tour = Application.Helper.Entities.Tour;

namespace Application.Helper.Helpers
{
    public class ApiHelper : DependentActions
    {
        private readonly string _baseUrl = Configuration.BaseUrl;
        private const string EndpointLocator = ApiConstants.EndpointLocator;
        private const string CosmosGuidTemplate = ApiConstants.CosmosGuidTemplate;
        private const string TimelineApiServiceUrl = ApiConstants.TimelineApiServiceUrl;
        private const string ExhibitApiServiceUrl = ApiConstants.ExhibitApiServiceUrl;
        private const string TourApiServiceUrl = ApiConstants.TourApiServiceUrl;

        public Guid CreateTimelineByApi(Timeline timeline)
        {
            timeline.Timeline_ID = new Guid(CosmosGuidTemplate);

            DataContractJsonSerializer timelineSerializer = new DataContractJsonSerializer(typeof(Timeline));
            DataContractJsonSerializer guidSerializer = new DataContractJsonSerializer(typeof(Guid));


            HttpWebRequest request = MakePutRequest(TimelineApiServiceUrl);
            Stream requestStream = request.GetRequestStream();
            timelineSerializer.WriteObject(requestStream, timeline);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                throw new NullReferenceException("responseStream is null");
            }

            Guid timelineId = (Guid)guidSerializer.ReadObject(responseStream);
            return timelineId;
        }

        public void DeleteTimelineByApi(Timeline timeline)
        {
            timeline.Timeline_ID = new Guid(CosmosGuidTemplate);
            DataContractJsonSerializer timelineSerializer = new DataContractJsonSerializer(typeof(Timeline));

            HttpWebRequest request = MakeDeleteRequest(TimelineApiServiceUrl);
            Stream requestStream = request.GetRequestStream();
            timelineSerializer.WriteObject(requestStream, timeline);
            request.GetResponse();
        }

        public NewExhibitApiResponse CreateExhibitByApi(Exhibit exhibit)
        {
            DataContractJsonSerializer exhibitSerializer = new DataContractJsonSerializer(typeof(Exhibit));
            DataContractJsonSerializer guidSerializer = new DataContractJsonSerializer(typeof(NewExhibitApiResponse));

            exhibit.Timeline_ID = new Guid(CosmosGuidTemplate);

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    exhibitSerializer.WriteObject(ms, exhibit);
            //    var ttt = Encoding.Default.GetString(ms.ToArray());
            //}

            HttpWebRequest request = MakePutRequest(ExhibitApiServiceUrl);
            Stream requestStream = request.GetRequestStream();
            exhibitSerializer.WriteObject(requestStream, exhibit);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            if (responseStream == null)
            {
                throw new NullReferenceException("responseStream is null");
            }

            NewExhibitApiResponse newExhibitApiResponse = (NewExhibitApiResponse)guidSerializer.ReadObject(responseStream);
            return newExhibitApiResponse;
        }

        public void DeleteExhibitByApi(Exhibit exhibit)
        {
            DataContractJsonSerializer exhibitSerializer = new DataContractJsonSerializer(typeof(Exhibit));

            HttpWebRequest request = MakeDeleteRequest(ExhibitApiServiceUrl);
            Stream requestStream = request.GetRequestStream();
            exhibitSerializer.WriteObject(requestStream, exhibit);
            request.GetResponse();
        }

        public void DeleteTourByApi(Tour tour)
        {
            DataContractJsonSerializer tourSerializer = new DataContractJsonSerializer(typeof(Tour));
            HttpWebRequest request = MakeDeleteRequest(TourApiServiceUrl);
            Stream requestStream = request.GetRequestStream();
            tourSerializer.WriteObject(requestStream, tour);
            request.GetResponse();
        }

        private HttpWebRequest MakePutRequest(string serviceUrl)
        {
            string endPoint = String.Format(EndpointLocator, _baseUrl, serviceUrl);
            return CreateRequest(endPoint, "PUT");
        }

        private HttpWebRequest MakeDeleteRequest(string serviceUrl)
        {
            string endPoint = String.Format(EndpointLocator, _baseUrl, serviceUrl);
            return CreateRequest(endPoint, "DELETE");
        }

        private static HttpWebRequest CreateRequest(string endPoint, string method)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "application/json, text/javascript, */*";
            request.Method = method;
            return request;
        }
    }
}