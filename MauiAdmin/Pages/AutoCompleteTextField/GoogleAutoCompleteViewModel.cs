﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Globalization;
using UraniumUI;

namespace MauiAdmin.Pages.AutoCompleteTextField;
public class GoogleAutoCompleteViewModel : UraniumBindableObject
{
    private string searchtext;

    public string Searchtext { get => searchtext; set => SetProperty(ref searchtext, value, doAfter:UpdateSuggestions); }

    private IEnumerable<string> suggestions;
    public IEnumerable<string> Suggestions { get => suggestions; set => SetProperty(ref suggestions, value); }
    
    private HttpClient httpClient = new HttpClient();
    
    private async void UpdateSuggestions(string value)
    {
        Suggestions = (await GetSuggestionsAsync(value)).ToList();
    }

    private async Task<IEnumerable<string>> GetSuggestionsAsync(string text)
    {
        try
        {
            var response = await httpClient.GetAsync($"https://www.baidu.com/s?wd={text}&hl={CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}&client=firefox");
            if (!response.IsSuccessStatusCode)
                return null;
            var bytes = await response.Content.ReadAsByteArrayAsync();
            //var result = JsonConvert.DeserializeObject<JArray>(System.Text.Encoding.UTF8.GetString(bytes));
            List<string> suggestions = new List<string>();
            for (global::System.Int32 i = 0; i < 100; i++)
            {
                suggestions.Add(i.ToString());
            }
            return suggestions;// result[1].Select(s => ((JValue)s)?.Value?.ToString());
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
    }
}
