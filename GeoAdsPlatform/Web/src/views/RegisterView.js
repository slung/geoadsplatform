(function( GA )
{
	//Selectors
	var EMAIL_INPUT_SELECTOR = "#register-email";
	var PASSWORD_INPUT_SELECTOR = "#register-pass";
	var PASSWORD_CONFIRM_INPUT_SELECTOR = "#register-pass-confirm";
	var ERROR_MESSAGE_SELECTOR = ".error-message";
	
	//Classes
	var INPUT_ERROR_CLASS = "input-error";
	
	//Messages
	var INVALID_EMAIL_ERROR_MSG = "The e-mail is not valid!";
	var INVALID_PASSWORD_ERROR_MSG = "The password is not valid!";
	var PASSWORDS_MISMATCH_ERROR_MSG = "The passwords do not match!";
	
	var RegisterView = GA.View.extend({
		
		events: {
			"#registerBtn":{
				click: "onRegisterSubmitClick"
			},
			".login-icon":{
				click: "onHomeClick"
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
			this.container.innerHTML = this.mustache( this.templates.main, {
				errorMessage: this.errorMessage
			});
			
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
		
		getConfirmationPassword: function()
		{
			return GA.one( PASSWORD_CONFIRM_INPUT_SELECTOR, this.container ).value;
		},
		
		samePasswords: function()
		{
			var password = this.getPassword();
			var confirmationPassword = this.getConfirmationPassword();
			
			if ( password == confirmationPassword )
				return true;
			
			return false;
		},
		
		/*
		 * Messages
		 */
		
		
		/*
		 * Events
		 */
		
		onRegisterSubmitClick: function( evt )
		{
			var emailValid = false;
			var passwordValid = false;
			
			if ( !GA.validateEmailInput( this.getEmail() ) )
			{
				GA.addClass( GA.one( EMAIL_INPUT_SELECTOR, this.container ), INPUT_ERROR_CLASS );
				emailValid = false;
				this.errorMessage = INVALID_EMAIL_ERROR_MSG;
				
				this.render();
				return;		
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
				this.errorMessage = INVALID_PASSWORD_ERROR_MSG;
				
				this.render();
				return;
			}
			else
			{
				GA.removeClass( GA.one( PASSWORD_INPUT_SELECTOR, this.container ), INPUT_ERROR_CLASS );
				passwordValid = true;
			}
			
			if ( !GA.validatePasswordInput( this.getConfirmationPassword() ) )
			{
				GA.addClass( GA.one( PASSWORD_CONFIRM_INPUT_SELECTOR, this.container ), INPUT_ERROR_CLASS );
				passwordValid = false;		
				this.errorMessage = INVALID_PASSWORD_ERROR_MSG;
				
				this.render();
				return;
			}
			else
			{
				GA.removeClass( GA.one( PASSWORD_CONFIRM_INPUT_SELECTOR, this.container ), INPUT_ERROR_CLASS );
				passwordValid = true;
			}
			
			if ( !this.samePasswords() )
			{
				this.errorMessage = PASSWORDS_MISMATCH_ERROR_MSG;
				this.render();
				return;
			}
			else
			{
				
			}
		},
		
		onHomeClick: function( evt )
		{
			//Redirect to home and change state
			window.location.href = "index.html";
			this.sendMessage("changeState", { state: GA.App.States.HOME });
		}
		
	});
	
	// Publish
	GA.RegisterView = RegisterView;
	
}(GA));