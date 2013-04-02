(function( GA )
{
	var EMAIL_INPUT_SELECTOR = "#login-email";
	var PASSWORD_INPUT_SELECTOR = "#login-pass";
	var INPUT_ERROR_CLASS = "input-error";
	var LOGIN_ERROR_MSG = "";
	
	var LoginView = GA.View.extend({
		
		events: {
			"#loginBtn":{
				click: "onLoginSubmitClick"
			},
			"#lostBtn":{
				click: "onLostPasswordClick"
			},
			".login-icon":{
				click: "onHomeClick"
			},
			"#registerBtn":{
				click: "onRegisterClick"
			}
		},
		
		init: function( cfg ) {
			
			// Call super
			this._parent( cfg );
		},
		
		register: function()
		{
		},
		
		render: function( template )
		{
			this.container.innerHTML = this.mustache( this.templates.main, {});
			
			return this;
		},
		
		getEmail: function()
		{
			return GA.one( EMAIL_INPUT_SELECTOR, this.container ).value;
		},
		
		getPassword: function()
		{
			return GA.one( PASSWORD_INPUT_SELECTOR, this.container ).value;
		},
		
		/*
		 * Messages
		 */
		
		
		/*
		 * Events
		 */
		
		onLoginSubmitClick: function( evt )
		{
			var emailValid = false;
			var passwordValid = false;
			
			if ( !GA.validateEmailInput( this.getEmail() ) )
			{
				GA.addClass( GA.one( EMAIL_INPUT_SELECTOR, this.container ), INPUT_ERROR_CLASS );
				emailValid = false;				
			}
			else
			{
				GA.removeClass( GA.one( EMAIL_INPUT_SELECTOR, this.container ), INPUT_ERROR_CLASS );
				emailValid = true;
			}
				
			if ( !GA.validatePasswordInput( this.getPassword() ) )
			{
				GA.addClass( GA.one( PASSWORD_INPUT_SELECTOR, this.container ), INPUT_ERROR_CLASS );
				passwordValid = false;				
			}
			else
			{
				GA.removeClass( GA.one( PASSWORD_INPUT_SELECTOR, this.container ), INPUT_ERROR_CLASS );
				passwordValid = true;
			}
			
			if ( !emailValid || !passwordValid )
				return;
				
			//Call login system
			this.ajax.login( this.getEmail(), this.getPassword(), function(){
				//On success, hide Sign in and Sign up labels on Menu
			}, function(){
				//On error, activate error msg
			} );
		},
		
		onHomeClick: function( evt )
		{
			//Redirect to Home page and change state
			window.location.href = "index.html";
			this.sendMessage("changeState", { state: GA.App.States.HOME });
		},
		
		onRegisterClick: function( evt )
		{
			//Redirect to Register page and change state
			window.location.href = "register.html";
			this.sendMessage("changeState", { state: GA.App.States.REGISTER });
		}
		
	});
	
	// Publish
	GA.LoginView = LoginView;
	
}(GA));