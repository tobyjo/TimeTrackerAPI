using System.Text.Json;

public class TimeTrackerDataStore
{
    public RootDto Data { get; }

    public TimeTrackerDataStore()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        Data = JsonSerializer.Deserialize<RootDto>(JsonData, options) ?? new RootDto();
    }


    private const string JsonData = @"{
      ""Project"": [
      {
        ""ID"": 1,
       ""Code"": ""PRJ001"",
       ""Description"": ""Project Alpha""
      },
      {
           ""ID"": 2,
       ""Code"": ""PRJ002"",
       ""Description"": ""Project Beta""
      },
      {
        ""ID"": 3,
       ""Code"": ""PRJ003"",
       ""Description"": ""Project Gamma""
      }
     ],
 ""SegmentType"": [
  {
   ""ID"": 1,
   ""Name"": ""Meeting""
  },
  {
   ""ID"": 2,
   ""Name"": ""Calls""
  },
  {
   ""ID"": 3,
   ""Name"": ""Documentation""
  }
 ],
 ""User"": [
  {
   ""UserID"": 101,
   ""UserName"": ""jdoe"",
   ""FullName"": ""John Doe""
  },
  {
   ""UserID"": 102,
   ""UserName"": ""asmith"",
   ""FullName"": ""Alice Smith""
  }
 ],
 ""TimeEntry"": [
  {
   ""StartDateTime"": ""2024-06-01T09:00:00Z"",
   ""EndDateTime"": ""2024-06-01T17:00:00Z"",
     ""ProjectID"": 1,
   ""SegmentTypeID"": 1,
   ""UserID"": 101
  },
  {
   ""StartDateTime"": ""2024-06-02T09:00:00Z"",
   ""EndDateTime"": ""2024-06-02T17:00:00Z"",
     ""ProjectID"": 1,
   ""SegmentTypeID"": 2,
   ""UserID"": 101
  },
  {
   ""StartDateTime"": ""2024-06-03T09:00:00Z"",
   ""EndDateTime"": ""2024-06-03T17:00:00Z"",
     ""ProjectID"": 1,
   ""SegmentTypeID"": 3,
   ""UserID"": 102
  }
 ]  
}";

}