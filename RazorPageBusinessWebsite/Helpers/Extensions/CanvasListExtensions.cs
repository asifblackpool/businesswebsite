using Content.Modelling.Models.Canvas.Lists;
using Newtonsoft.Json;

namespace RazorPageBusinessWebsite.Helpers.Extensions
{
    /// <summary>
    /// Helper extension methods for rendering lists
    /// </summary>
    public static class CanvasListExtensions
    {
        /// <summary>
        /// Determines if the list should be rendered as ordered (numbered)
        /// </summary>
        public static bool IsOrdered(this CanvasListBlock list)
        {
            return list?.Properties?.ListType?.ToLower() == "ordered";
        }

        /// <summary>
        /// Gets the HTML tag name for the list
        /// </summary>
        public static string GetHtmlTag(this CanvasListBlock list)
        {
            return list.IsOrdered() ? "ol" : "ul";
        }

        /// <summary>
        /// Renders the list as HTML with proper tags and CSS classes
        /// </summary>
        public static string RenderHtml(this CanvasListBlock list, string cssClass = "shade-black")
        {
            if (list?.Value == null || list.Value.Count == 0)
                return string.Empty;

            var html = new System.Text.StringBuilder();
            string tag = list.GetHtmlTag();

            html.Append($"<{tag} class=\"{cssClass}\">");

            foreach (var item in list.Value)
            {
                string content = ExtractTextContent(item.Content);
                html.Append($"<li>{content}</li>");
            }

            html.Append($"</{tag}>");
            return html.ToString();
        }

        /// <summary>
        /// Renders the list as HTML with support for nested lists
        /// </summary>
        public static string RenderHtmlRecursive(this CanvasListBlock list, string cssClass = "shade-black")
        {
            if (list?.Value == null || list.Value.Count == 0)
                return string.Empty;

            var html = new System.Text.StringBuilder();
            string tag = list.GetHtmlTag();
            html.Append($"<{tag} class=\"{cssClass}\">");

            foreach (var item in list.Value)
            {
                string content = ExtractContentWithNestedList(item);
                html.Append($"<li>{content}</li>");
            }

            html.Append($"</{tag}>");
            return html.ToString();
        }

        private static string ExtractContentWithNestedList(CanvasListItem item)
        {
            return ExtractTextContent(item.Content);
        }

        /// <summary>
        /// Extracts text/HTML from IValue (handles SimpleValue, ComplexValue with all fragment types)
        /// </summary>
        private static string ExtractTextContent(IValue? value)
        {
            if (value == null)
                return string.Empty;

            if (value is SimpleValue simple)
            {
                return simple.Text ?? string.Empty;
            }
            else if (value is ComplexValue complex)
            {
                var fragments = new List<string>();
                foreach (var fragment in complex.Fragments)
                {
                    fragments.Add(RenderFragment(fragment));
                }
                return string.Join("", fragments);
            }

            return string.Empty;
        }

        /// <summary>
        /// Renders a single fragment to HTML string
        /// </summary>
        private static string RenderFragment(ContentFragment fragment)
        {
            if (fragment == null)
                return string.Empty;

            // Check for LinkFragment
            if (fragment is LinkFragment link)
            {
                string linkText = link.Text ?? string.Empty;
                string url = link.Url ?? "#";
                return $"<a href=\"{url}\">{linkText}</a>";
            }

            // Check for HtmlFragment
            if (fragment is HtmlFragment html)
            {
                return html.HtmlContent ?? string.Empty;
            }

            // Check for ImageFragment
            if (fragment is ImageFragment image)
            {
                return $"<img src=\"{image.Source}\" alt=\"{image.AltText}\" />";
            }

            // Check for TextFragment
            if (fragment is TextFragment text)
            {
                return text.Text ?? string.Empty;
            }

            // Check by type string as fallback
            if (fragment.Type == "_link")
            {
                string linkText = fragment.Text ?? string.Empty;
                string url = fragment.Properties?.Link?.System?.Uri ?? "#";
                return $"<a href=\"{url}\">{linkText}</a>";
            }

            // Fallback for any other fragment type
            return fragment.Text ?? string.Empty;
        }

        /// <summary>
        /// Deserializes a full _list block from JSON
        /// </summary>
        public static CanvasListBlock? DeserializeListBlock(string json)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new ValueConverter() },
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.DeserializeObject<CanvasListBlock>(json, settings);
        }
    }
}