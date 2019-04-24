// <copyright file="JsonContent.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace App.Metrics.Health.Reporting.Slack.Internal
{
    /// <summary>
    ///     Provides HTTP content based on a json string.
    /// </summary>
    internal class JsonContent : StringContent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonContent" /> class.
        /// </summary>
        /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.StringContent" />.</param>
        public JsonContent(object content)
            : base(JsonConvert.SerializeObject(content, DefaultJsonSerializerSettings.Instance), Encoding.UTF8, "application/json")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonContent" /> class.
        /// </summary>
        /// <param name="content">
        ///     The content used to initialize the <see cref="T:System.Net.Http.JsonContent" />.
        /// </param>
        /// <param name="encoding">The encoding to use for the content.</param>
        public JsonContent(object content, Encoding encoding)
            : base(JsonConvert.SerializeObject(content, DefaultJsonSerializerSettings.Instance), encoding, "application/json")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonContent" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="setting">The JSON serializer setting to use</param>
        public JsonContent(object content, Encoding encoding, string mediaType, JsonSerializerSettings setting)
            : base(JsonConvert.SerializeObject(content, setting), encoding, mediaType)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonContent" /> class.
        /// </summary>
        /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.StringContent" />.</param>
        /// <param name="setting">The JSON serializer setting to use</param>
        public JsonContent(object content, JsonSerializerSettings setting)
            : base(JsonConvert.SerializeObject(content, setting), Encoding.UTF8, "application/json")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonContent" /> class.
        /// </summary>
        /// <param name="content">
        ///     The content used to initialize the <see cref="T:System.Net.Http.JsonContent" />.
        /// </param>
        /// <param name="encoding">The encoding to use for the content.</param>
        /// <param name="setting">The JSON serializer setting to use</param>
        public JsonContent(object content, Encoding encoding, JsonSerializerSettings setting)
            : base(JsonConvert.SerializeObject(content, setting), encoding, "application/json")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonContent" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mediaType">Type of the media.</param>
        public JsonContent(object content, Encoding encoding, string mediaType)
            : base(JsonConvert.SerializeObject(content, DefaultJsonSerializerSettings.Instance), encoding, mediaType)
        {
        }
    }
}