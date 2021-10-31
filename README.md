# TrucksApi
An ASP.NET Core Web API for adding and retrieving food trucks.  

## Included
- TrucksApi.csproj
- TrucksApi.Test.csproj

## Not Included
The API is inteded to be part of a larger solution.  The vision being that a region\municipality could:
- call this API to map\add food truck data in the expected format
- create various client or admin applications to consume the data

## Usage
The TrucksApi has the following endpoints (see Swagger for details):
- GET api/trucks?zone={p1}&zoneType={p2}
- GET api/admin/trucks
- GET api/admin/trucks/{id}
- POST api/admin/truck

## Current Considerations and Assumptions
- The required operations are split into user (default) and admin endpoints under assumption that the only requirement for the user at this time is to retrieve a filtered set of data, and that admin endpoint would be secured.
- I can see no reason at this time for regions\municipalities to coexist in same data store, so I would suggest that data be partitioned by source. And if partitioned, might be reasonable to allow admin (as a low volumn use case) to retrieve all results without paging?
- I do see a reason for the consuming client to want multiple regions\municipalities to coexist on the client, but that may be managed on the client? TBD, might it be helpful to identify in the data model where the data came from (source and region represented?).
- Assume generic\simple data model for now (grow organically based on feedback). Description field itself can of course hold anything deemed important.  
- Would want more discussions around the inital load of data, e.g.:
  - Where might it be initiated?  Admin UI? User UI (e.g. load data for region)? Automated such as from timer or trigger from other workflow?  
  - How often? What is the cadence of change?  What is range of acceptable update interval?  Peak and non-peak usage times?

## Future Considerations
- Search
  - Consider criteria other than zone, e.g. neighborhood, location-hierarchy, cuisine, hours, open now, favorites, etc
  - Admin PUT and DELETE actions
- Add 
  - Bulk insert support? 
  - What other generic model attributes would be common and helpful?
- Admin PUT and DELETE endpoints 
- Anonymous feedback API to regions\municipalities? E.g. (checkin, favorites, logging, history, submit corrections)

