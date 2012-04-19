using System;
using System.Collections.Generic;
using System.IO;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

namespace Suteki.Common.Services
{
	public class EmailBuilder : IEmailBuilder
	{
		readonly VelocityEngine velocityEngine;
		//Default path for email templates
		const string defaultTemplatePath = "Views\\EmailTemplates";

		/// <summary>
		/// Creates an instance of the EmailBuilder using the specified template path (relative to the application root)
		/// </summary>
		/// <param name="templatePath">The path to the templates, relative to the application root</param>
		public EmailBuilder(string templatePath) : this(CreateDefaultProperties(templatePath))
		{
		}

		/// <summary>
		/// Creates an instance of the EmailBuilder using the default template path (Views\EmailTemplates)
		/// </summary>
		public EmailBuilder() : this(CreateDefaultProperties(defaultTemplatePath))
		{
		}

		/// <summary>
		/// Creates an instance of the EmailBuilder using the specified properties.
		/// </summary>
		/// <param name="nvelocityProperties"></param>
		public EmailBuilder(IDictionary<string, object> nvelocityProperties)
		{
			var properties = new ExtendedProperties();

			foreach (var pair in nvelocityProperties)
			{
				properties.AddProperty(pair.Key, pair.Value);
			}

			velocityEngine = new VelocityEngine();
			velocityEngine.Init(properties);
		}

		private static IDictionary<string, object> CreateDefaultProperties(string templatePath) 
		{
			var defaultProperties = new Dictionary<string, object>();
			string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatePath);
			defaultProperties.Add(RuntimeConstants.RESOURCE_LOADER, "file");
			defaultProperties.Add(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, basePath);
			return defaultProperties;
		}

		public string GetEmailContent(string templateName, IDictionary<string, object> viewdata)
		{
			return BuildEmail(templateName, viewdata);
		}

		/// <exception cref="ArgumentException"></exception>
		string BuildEmail(string templateName, IDictionary<string, object> viewdata)
		{
			if (viewdata == null)
			{
				throw new ArgumentNullException("viewData");
			}

			if (string.IsNullOrEmpty(templateName))
			{
				throw new ArgumentException("TemplateName");
			}

			var template = ResolveTemplate(templateName);

			var context = new VelocityContext();

			foreach (var key in viewdata.Keys)
			{
				context.Put(key, viewdata[key]);
			}

			using (var writer = new StringWriter())
			{
				template.Merge(context, writer);
				return writer.ToString();
			}
		}

		Template ResolveTemplate(string name)
		{
			if (!Path.HasExtension(name))
			{
				name += ".vm";
			}

			if (!velocityEngine.TemplateExists(name))
			{
				throw new InvalidOperationException(string.Format("Could not find a template named '{0}'", name));
			}

			return velocityEngine.GetTemplate(name);
		}
	}
}