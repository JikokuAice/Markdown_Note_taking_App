using System.Text.Json.Serialization;

namespace Markdown_Note_taking_App.Model
{

    /*    "edits": [
            {
                "description": "Punctuation error.",
                "end": 2,
                "error_type": "M:PUNCT",
                "general_error_type": "Punctuation",
                "id": "8ead3d09-bdd1-5c3c-a80d-4f62d1137f47",
                "replacement": "Hi,",
                "sentence": "Hi how are you.",
                "sentence_start": 0,
                "start": 0
            },
    */
    public class GrammerCheckResponse
    {
        public Edit[] edits { get; set; }

    }



    public class Edit
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("error_type")]
        public string ErrorType { get; set; }

        [JsonPropertyName("replacement")]
        public string Replacement { get; set; }
        [JsonPropertyName("sentence")]
        public string sentence { get; set; }

    }

}
