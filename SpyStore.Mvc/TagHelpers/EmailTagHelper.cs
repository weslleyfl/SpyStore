using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyStore.Mvc.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        //<email email-name="blog" email-domain="skimedic.com"></email>
        public string EmailName { get; set; }
        public string EmailDomain { get; set; }

        /// <summary>
        /// When a Tag Helper is invoked, the Process method is called. The Process
        /// method takes two parameters, a TagHelperContext and a TagHelperOutput.The
        /// TagHelperContext is used to get any other attributes on the tag and a dictionary of
        /// objects used to communicate with other tag helpers targeting child elements.
        /// The TagHelperOutput is used to create the rendered output
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a"; // Replaces <email> with <a> tag
            var address = EmailName + "@" + EmailDomain;
            output.Attributes.SetAttribute("href", "mailto:" + address);
            output.Content.SetContent(address);
        }

    }
}
