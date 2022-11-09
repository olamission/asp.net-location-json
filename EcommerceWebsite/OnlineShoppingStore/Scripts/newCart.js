document.addEventListener('DOMContentLoaded', () => {
    document.getElementById("Checkout").addEventListener('click', function () {
        var maxNum = document.getElementById("maxNum").innerHTML;
        var newCart = [];
        for (let i = 0; i < maxNum; i++) {
            var quantityVal = document.getElementById("QuantityCheckout" + i);
            var quantityProd = document.getElementById("QuantityProduct" + i);
            console.log(quantityVal.value);
            console.log(quantityProd.innerHTML);
            newCart.push({
                "product": quantityProd.innerHTML,
                "value": quantityVal.value
            });
        }
        console.log(newCart);
        //newCart = JSON.stringify({ 'newCart' : newCart});
        $.ajax({
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(newCart),
            url: '/Home/GetValue'
        }).done(function (data) {
            console.log('done');
        });
    });


    
    
});
//map
setTimeout(function () {
    window.dispatchEvent(new Event('resize'));
    var distibutionIcon = L.ExtraMarkers.icon({
        icon: 'fa-bag-shopping',
        markerColor: 'red',
        shape: 'square',
        prefix: 'fa'
    });
    var map = L.map('map').setView([51.505, -0.09], 12);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 12,
        attribution: '© OpenStreetMap'
    }).addTo(map);
    if (navigator.geolocation) { //check if geolocation is available
        navigator.geolocation.getCurrentPosition(function (position) {
            let lat = position.coords.latitude;
            let lon = position.coords.longitude;
            var marker = L.marker([lat -0.008 , lon + 0.1  ]).addTo(map);
            map.setView([lat - 0.008, lon + 0.1], 12);
            /*var popup = L.popup();
            popup
                .setLatLng([lat , lon])
                .setContent("")
                .openOn(map);
            */
            //Your location on the map at latitude: " + lat.toString() + " and longitude : " + lon.toString()
        })
        //creates markers for the distribution centers
        $.ajax({
            method: "GET",
            url: '/Home/GetStores'
        }).done(function (data) {
            console.log('done');
            console.log(JSON.parse(data));
            var StoreListObject = JSON.parse(data);
            for (let i = 0; i < StoreListObject["storeList"].length; i++ ) {
                var name = StoreListObject["storeList"][i]["name"];
                var lat = StoreListObject["storeList"][i]["latitude"];
                var long = StoreListObject["storeList"][i]["longitude"];
                console.log(name);
                cityMarker = L.marker([lat , long ], { icon: distibutionIcon }).addTo(map);
            }
        });
    }
}, 1000);


