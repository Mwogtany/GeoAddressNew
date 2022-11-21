﻿$(document).ready(function () {
    function geoFindMe() {
        var status = document.querySelector('#status');
        var mapLink = document.querySelector('#map-link');

        mapLink.href = '';
        mapLink.textContent = '';

        function success(position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;

            status.textContent = '';
            mapLink.href = 'https://www.openstreetmap.org/#map=18/${latitude}/${longitude}';
            mapLink.textContent = 'Latitude: ${latitude} °, Longitude: ${longitude} °';
        }

        function error() {
            status.textContent = 'Unable to retrieve your location';
        }

        if (!navigator.geolocation) {
            status.textContent = 'Geolocation is not supported by your browser';
        } else {
            status.textContent = 'Locating…';
            navigator.geolocation.getCurrentPosition(success, error);
        }

    }


    //**************************************************************************************************
    //document.querySelector('#find-me').addEventListener('click', geoFindMe);
    //function getLocation() {
    //    if (navigator.geolocation) {
    //        navigator.geolocation.getCurrentPosition(showPosition, showError);
    //    }
    //    else { x.innerHTML = "Geolocation is not supported by this browser."; }
    //}

    //function showPosition(position) {
    //    var latlondata =  position.coords.latitude + "," +position.coords.longitude;
    //    var latlon = "Your Latitude Position is:=" + position.coords.latitude + "," + "Your Longitude Position is:="  +position.coords.longitude;
    //    y.innerHTML(latlon)
    //    var img_url = "http://maps.googleapis.com/maps/api/staticmap?center="
    //        + latlondata + "&zoom=14&size=400x300&sensor=false";
    //    x.innerHTML = "<img src='" + img_url + "' />";
    //}
    //function showError(error) {
    //    if (error.code == 1) {
    //        y.innerHTML = "User denied the request for Geolocation."
    //    }
    //    else if (err.code == 2) {
    //        y.innerHTML = "Location information is unavailable."
    //    }
    //    else if (err.code == 3) {
    //        y.innerHTML = "The request to get user location timed out."
    //    }
    //    else {
    //        y.innerHTML = "An unknown error occurred."
    //    }
    //}

    //if (navigator.geolocation) {
    //    navigator.geolocation.getCurrentPosition(success);
    //} else {
    //    alert("Geo Location is not supported on your current browser!");
    //}

    //function success(position) {
    //    var latitude = position.coords.latitude;
    //    var longitude = position.coords.longitude;
    //var xlat = document.getElementById('<%=Latitude.ClientID%>').value;
    //var ylong = document.getElementById('<%=Longitude.ClientID%>').value;
    //var mEmpty = "";
    //if (xlat == "" || xlat == null || xlat == 'null' || xlat == 'none' || xlat == 'n/a' || xlat == 'N/A' || xlat == '-') {
    //   mEmpty = "x";
    //document.getElementById('<%=Latitude.ClientID%>').value = latitude;
    // }
    // else {
    //     latitude = xlat;
    // }

    //if (ylong == "" || ylong == null || ylong == 'null' || ylong == 'none' || ylong == 'n/a' || ylong == 'N/A' || ylong == '-') {
    //document.getElementById('<%=Longitude.ClientID%>').value = longitude;
    // }
    // else {
    //     longitude = ylong;
    //  }
    //    var marker = new google.maps.Marker({
    //        position: new google.maps.LatLng(longitude, latitude)
    //    });

    //    var city = position.coords.locality;
    //    var myLatlng = new google.maps.LatLng(latitude, longitude);
    //    var myOptions = {
    //        center: myLatlng,
    //        zoom: 14,
    //        mapTypeId: google.maps.MapTypeId.ROADMAP
    //    };

    //    var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    //    google.maps.event.addListener(map, 'click', function (e) {
    //        alert("Latitude: " + e.latLng.lat() + "\r\nLongitude: " + e.latLng.lng());
    //    });
    //    var marker = new google.maps.Marker({
    //        position: myLatlng,
    //        title: "lat: " + latitude + " long: " + longitude
    //    });
    //    //var mEmpty = "";
    //    if (mEmpty == "") {
    //        marker.setMap(map);
    //        var infowindow = new google.maps.InfoWindow({ content: "<b>School Address</b><br/> Latitude:" + latitude + "<br /> Longitude:" + longitude + "" });
    //        infowindow.open(map, marker);
    //    }
    //    else {
    //        marker.setMap(map);
    //        var infowindow = new google.maps.InfoWindow({ content: "<b>Current Address</b><br/> Latitude:" + latitude + "<br /> Longitude:" + longitude + "" });
    //        infowindow.open(map, marker);
    //    }
    ////*********************************************************************************************************************

    //}
    // 2*********************************************************************************************************************
    //window.onload = function () {
    //var xlat = document.getElementById('<%=Latitude.ClientID%>').value;
    //var ylong = document.getElementById('<%=Longitude.ClientID%>').value;

    //alert("Latitude: " + xlat + "\r\nLongitude: " + ylong);
    //    var myLatlng = new google.maps.LatLng(xlat, ylong);
    //    var mapOptions = {
    //        center: new google.maps.LatLng(xlat, ylong),
    //        zoom: 14,
    //        mapTypeId: google.maps.MapTypeId.ROADMAP
    //    };
    //   var infoWindow = new google.maps.InfoWindow();
    //    var latlngbounds = new google.maps.LatLngBounds();
    //    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
    //    google.maps.event.addListener(map, 'click', function (e) {
    //        alert("Latitude: " + e.latLng.lat() + "\r\nLongitude: " + e.latLng.lng());
    //    });
    //    var marker = new google.maps.Marker({
    //        position: myLatlng,
    //        title: "lat: " + xlat + " long: " + ylong
    //    });
    //    marker.setMap(map);
    //    var infowindow = new google.maps.InfoWindow({ content: "<b>School</b><br/> Latitude:" + xlat + "<br /> Longitude:" + ylong + "" });
    //    infowindow.open(map, marker);
    //}
    //************************************************************************************************************************
    var map;
    var infowindow = null;
    var allowedZoomLevel = 6;
    var allowedMapBounds = new google.maps.LatLngBounds(
            new google.maps.LatLng(5.2852443672086515, 42.036873727280614),     //Map boundaries of Kenya approximately
            new google.maps.LatLng(-5.107266569317371, 33.621346383530614)
        );

    function initMap() {
        //Enabling new cartography and themes
        var xlat = document.getElementById('Latitude').value;
        var ylong = document.getElementById('Longitude').value;
        var lat = null;
        var lng = null;

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
            function (position) {
                var lat = position.coords.latitude;
                var lng = position.coords.longitude;
            });
        }

        var mEmpty = "";
        if (xlat == "" || xlat == null || xlat == 'null' || xlat == 'none' || xlat == 'n/a' || xlat == 'N/A' || xlat == '-') {
            if (lat == "" || lat == null) {
                xlat = -1.289802
                ylong = 36.825629711
            }
            else {
                xlat = lat;
                ylong = lng;
            }
            mEmpty = "x";
        }

        google.maps.visualRefresh = true;
        //Setting starting options of map
        var mapOptions = {
            center: new google.maps.LatLng(xlat, ylong),
            zoom: 18,
            mapTypeId: google.maps.MapTypeId.HYBRID
            //mapTypeId: google.maps.MapTypeId.SATELLITE
            //mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        //Getting map DOM element
        var mapElement = document.getElementById('map_canvas');
        //Creating a map with DOM element which is just
        //obtained
        map = new google.maps.Map(mapElement, mapOptions);

        var wmsOptions = {
            baseUrl: 'http://demo.cubewerx.com/cubewerx/cubeserv.cgi?',
            layers: 'Foundation.gtopo30',
            version: '1.1.1',
            styles: 'default',
            format: 'image/png'
        };

        google.maps.event.addListener(map, 'click', function (e) {
            //alert("Latitude: " + e.latLng.lat() + "\r\nLongitude: " + e.latLng.lng());
            if (mEmpty != "") {
                document.getElementById('Latitude').value = e.latLng.lat();
                document.getElementById('Longitude').value = e.latLng.lng();
                //Use Google API to retrieve PlusCodes
                var myObj, x, txt = "";
                var actionUrl = 'https://plus.codes/api?address=' + e.latLng.lat() + ',' + e.latLng.lng() + '&email=mwogtany@gmail.com';
                $.getJSON(actionUrl, function (response) {
                    if (response != null) {
                        var x = 0;
                        $.each(response, function (key, value) {
                            if (value.global_code != null) {
                                document.getElementById('Pluscode').value = value.global_code;
                            }
                        });

                    }
                });
            }
        });
        google.maps.event.addListener(map, 'click', function (e) {
            if (infowindow != null)
                infowindow.close();
            infowindow = new google.maps.InfoWindow({
                content: '<b>Mouse Coordinates : </b><br><b>Latitude : </b>' + e.latLng.lat() + '<br><b>Longitude: </b>' + e.latLng.lng(),
                position: e.latLng
            });
            infowindow.open(map);
        });

        google.maps.event.addListener(map, 'drag', checkBounds);
        google.maps.event.addListener(map, 'zoom_changed', checkBounds);

        startButtonEvents();
        var myLatlng = new google.maps.LatLng(xlat, ylong);
        var marker = new google.maps.Marker({
            position: myLatlng,
            title: "lat: " + xlat + " long: " + ylong
        });

        //marker.setMap(map);
        //var infowindow = new google.maps.InfoWindow({ content: "<b>School</b><br/> Latitude:" + xlat + "<br /> Longitude:" + ylong + "" });
        //infowindow.open(map, marker);
        if (mEmpty == "") {
            marker.setMap(map);
            var infowindow = new google.maps.InfoWindow({ content: "<b>Address</b><br/> Latitude:" + xlat + "<br /> Longitude:" + ylong + "" });
            infowindow.open(map, marker);
        }
        else {
            marker.setMap(map);
            var infowindow = new google.maps.InfoWindow({ content: "<b>Current Address</b><br/> Latitude:" + xlat + "<br /> Longitude:" + ylong + "" });
            infowindow.open(map, marker);
        }

        var infowindow = new google.maps.InfoWindow({
            content: 'Marker Info Window – ID : ' + marker
        });

        google.maps.event.addListener(marker, 'click', function () {
            infowindow.open(map, marker);
        });
    }

    function checkBounds() {
        if (map.getZoom() < allowedZoomLevel)
            map.setZoom(allowedZoomLevel);

        //if (allowedMapBounds) {
        //    var allowedNELng = allowedMapBounds.getNorthEast().lng();
        //    var allowedNELat = allowedMapBounds.getNorthEast().lat();
        //    var allowedSWLng = allowedMapBounds.getSouthWest().lng();
        //    var allowedSWLat = allowedMapBounds.getSouthWest().lat();
        //    var recentBounds = map.getBounds();
        //    var recentNELng = recentBounds.getNorthEast().lng();
        //    var recentNELat = recentBounds.getNorthEast().lat();
        //    var recentSWLng = recentBounds.getSouthWest().lng();
        //    var recentSWLat = recentBounds.getSouthWest().lat();
        //    var recentCenter = map.getCenter();
        //    var centerX = recentCenter.lng();
        //    var centerY = recentCenter.lat();
        //    var nCenterX = centerX;
        //    var nCenterY = centerY;

        //    if (recentNELng > allowedNELng) centerX = centerX -(recentNELng - allowedNELng);
        //    if (recentNELat > allowedNELat) centerY = centerY -(recentNELat - allowedNELat);
        //    if (recentSWLng < allowedSWLng) centerX = centerX + (allowedSWLng - recentSWLng);
        //    if (recentSWLat < allowedSWLat) centerY = centerY + (allowedSWLat - recentSWLat);
        //    if (nCenterX != centerX || nCenterY != centerY) {
        //        map.panTo(new google.maps.LatLng(centerY, centerX));
        //    }
        //    else {
        //        return;
        //    }
        //}
    }

    function startButtonEvents() {
        document.getElementById('addStandardMarker').
        addEventListener('click', function () {
            addStandardMarker();
        });
        document.getElementById('addIconMarker').
        addEventListener('click', function () {
            addIconMarker();
        });
    }

    function startButtonEvents() {
        document.getElementById('btnRoad'
        ).addEventListener('click', function () {
            map.setMapTypeId(google.maps.MapTypeId.ROADMAP);
        });
        document.getElementById('btnSat'
        ).addEventListener('click', function () {
            map.setMapTypeId(google.maps.MapTypeId.SATELLITE);
        });
        document.getElementById('btnHyb'
        ).addEventListener('click', function () {
            map.setMapTypeId(google.maps.MapTypeId.HYBRID);
        });
        document.getElementById('btnTer'
        ).addEventListener('click', function () {
            map.setMapTypeId(google.maps.MapTypeId.TERRAIN);
        });
    }

    function createRandomLatLng() {
        var deltaLat = maxLat - minLat;
        var deltaLng = maxLng - minLng;
        var rndNumLat = Math.random();
        var newLat = minLat + rndNumLat * deltaLat;
        var rndNumLng = Math.random();
        var newLng = minLng + rndNumLng * deltaLng;
        return new google.maps.LatLng(newLat, newLng);
    }

    function addStandardMarker() {
        var coordinate = createRandomLatLng();
        var marker = new google.maps.Marker({
            position: coordinate,
            map: map,
            title: 'Random Marker - ' + markerId
        });
        // If you don't specify a Map during the initialization
        //of the Marker you can add it later using the line
        //below
        //marker.setMap(map);
        markerId++;
    }

    google.maps.event.addDomListener(window, 'load', initMap);
});