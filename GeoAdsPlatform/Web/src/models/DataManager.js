(function( GA )
{
	// Singleton instance
	var dataManager = null;
	var MASK_ELEMENT = ".page";
	
	var DataManager = GA.EventManager.extend({
		
		tableName: '',
		filters: [],
		rowCount: 20,
		cluster: null,
		showLoading: false,
		
		// stores loaded tables
		tables: [],
		
		init: function( cfg ) 
		{
			if( dataManager )
				throw new Error('You can only create one instance of DataManager!');
			
			this._parent();
		},
		
		geocode: function( address, multipleResults, options )
		{
			var options = options || {};
		    
			var geocoder = new google.maps.Geocoder();
		    
		    geocoder.geocode({ address: address }, GA.bind( function( results, status ) {
		    	
		    	if (status == google.maps.GeocoderStatus.OK) {
		    		
		    		if( multipleResults )
		    		{
		    			var addresses = [];
		    			var index = -1;
		    			
		    			for( var i=0; i<results.length;i++)
		    			{
							index++;
							
							addresses.push({
		    					index: index,
		    					lat: results[i].geometry.location.lat(),
		    					lon: results[i].geometry.location.lng(),
		    					address: results[i].formatted_address,
		    					bounds: [ 	results[i].geometry.viewport.getSouthWest().lat(), 
											results[i].geometry.viewport.getSouthWest().lng(),
											results[i].geometry.viewport.getNorthEast().lat(),
											results[i].geometry.viewport.getNorthEast().lng() ]
		    				});
		    				
		    			}
		    			
		    			if( options.success )
			    			options.success.apply( this, [addresses]);
		    		} 
		    		else
		    		{
		    			var lat = results[0].geometry.location.lat();
			    		var lon = results[0].geometry.location.lng();
			    		var formatted_address = results[0].formatted_address;
			    		var bounds = [ 	results[0].geometry.viewport.getSouthWest().lat(), 
										results[0].geometry.viewport.getSouthWest().lng(),
										results[0].geometry.viewport.getNorthEast().lat(),
										results[0].geometry.viewport.getNorthEast().lng() ];
			    		
			    		if( options.success )
			    			options.success.apply( this, [lat, lon, formatted_address, bounds]);
		    		}
		    	}
		    }, this));
		},
		
		reverseGeocode: function (lat, lng, options)
		{
			var options = options || {};
		    
			var geocoder = new google.maps.Geocoder();
		    var latLng = new google.maps.LatLng(lat, lng);
		    
		    geocoder.geocode({latLng: latLng}, GA.bind( function( results, status ) {
		    	
		    	if (status == google.maps.GeocoderStatus.OK) {
		    		
		    		var address = results[0].formatted_address;
		    		var bounds = results[0].geometry.viewport;
		    		
		    		if( options.success )
		    			options.success.apply( this, [address, bounds]);
		    	}
		    }, this));
		},
		
		/**
		 * Geolocates user and the using lat/lon makes a revers geocoding to
		 * get his address name. 
		 */
		geolocateUser: function()
		{
			if(navigator.geolocation)
			{
				navigator.geolocation.getCurrentPosition( 
					GA.bind( function( pos ) 
					{
						this.reverseGeocode(
					  		pos.coords.latitude, 
					  		pos.coords.longitude, 
					  		{
					  			success: GA.bind( function( address, googleBounds ) {
					  				
					  				this.geocodeBounds = [ 	googleBounds.getSouthWest().lat(), 
															googleBounds.getSouthWest().lng(),
															googleBounds.getNorthEast().lat(),
															googleBounds.getNorthEast().lng() ];
															
									this.geocodeCenter = [pos.coords.latitude, pos.coords.longitude];
									
									var msg = {
										lat: pos.coords.latitude,
										lon: pos.coords.longitude,
										address: address,
										bounds: this.geocodeBounds
									};

									this.fire('userGeocoded', msg);
									
					  			}, this )
					  		}
					  	);
					}, this), 
					GA.bind( function( error ) {
						this.fire('userNotGeocoded');
					}, this)
				);
			}
		}
		
	});
	
	DataManager.getInstance = function()
	{
		if( dataManager )
			return dataManager;
		
		dataManager = new DataManager();
		return dataManager;
	};
	
	// Publish
	GA.DataManager = DataManager;
	
}(GA));