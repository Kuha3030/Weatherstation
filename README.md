# Weatherstation
Weatherstation main-repo.

This repo contains:

Separate projects for weatherApis.
FMIAPI - responsible for handling data from Ilmatieteenlaitos.fi
YrnoAPI - responsible for handling data from Yr.No (https://api.met.no/)
ForecaAPI - responsible for handling data from foreca.fi

Geocode project is for maintaining geocode searching. This project is used to develope the GetGeocode.cs class.

citiesgeocode.csv is used (for now) to read most common finnish locations and their coordinates(geocodes).
Geocode-project is responsible for parsing this file and searching location if not found here.
This file will be moved to database later on.

Sasema-MVC folder contains the asp.NET MVC Web application 

EDIT: Separate projects are combined under the MVC application as classes in the final product.

MVC application needs a database to run properly.
