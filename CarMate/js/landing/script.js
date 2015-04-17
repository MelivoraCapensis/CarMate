jQuery(function() {
	$('input[placeholder], textarea[placeholder]').placeholder();
	$(".phone-mask").mask("+7 999 999 99 99");
	
	$("#slider").bjqs({
		width : 610,
		height : 220,
		showcontrols: true,
		centermarkers: true,
		nexttext: '',
		prevtext: ''
	});
	
	/*
	var nowTime = new Date();
	var weekday = nowTime.getDay();
	weekday = weekday == 0 ? 7 : weekday;
	var day_offset = 60 * 60 * 24 - (((nowTime.valueOf() / 1000) % (60 * 60 * 24)) + (3600 * 4));
	var diffSecs = Math.floor(((7 - weekday) * 1000 * 60 * 60 * 24) + (day_offset * 1000));
	dif(Math.floor(diffSecs / 1000));
	*/
	
	$('.reg-form').submit(function(e) {
		e.preventDefault();

		var $form = $(this),
				ok = true,
				$name = $form.find('.name'),
				$email = $form.find('.email'),
				$pwd = $form.find('.password');
		
		if ($name.length) {
			if ($name.hasClass('required')) {
				if (!$name.val()) {
					ok = false;
					$name.addClass("error");
				} else
					$name.removeClass("error");
			}
		}
		
		if ($pwd.hasClass('required')) {
			if (!$pwd.val()) {
				ok = false;
				$pwd.addClass("error");
			} else
				$pwd.removeClass("error");
		}

		if ($email.length) {
			if ($email.hasClass('required') || $email.val().length > 0) {
				if (!checkEmail($email.val())) {
					ok = false;
					$email.addClass("error");
				} else
					$email.removeClass("error");
			}
		}
		
		if (ok) {
			$form.ajaxSubmit({
				success: function(response) {
					hideModal();
					$form.get(0).reset();
					showModal('.thanks1-modal');
					setTimeout('hideModal()',3000);
					if ($('body').data('metrika') && window['yaCounter'+$('body').data('metrika')]) window['yaCounter'+$('body').data('metrika')].reachGoal('request');
					if (window._gaq) _gaq.push(['_trackEvent','Form','Sent']);
				}
			});
		}
	});
	
	$('.callback-form').submit(function(e) {
		e.preventDefault();

		var $form = $(this),
				ok = true,
				$name = $form.find('.name'),
				$phone = $form.find('.phone');
		
		if ($name.hasClass('required')) {
			if (!$name.val()) {
				ok = false;
				$name.addClass("error");
			} else
				$name.removeClass("error");
		}
		
		if ($phone.hasClass('required') && !/\d{11}/.test($phone.val().replace(/\D/g, ''))) {
			ok = false;
			$phone.addClass("error");
		} else
			$phone.removeClass("error");

		
		if (ok) {
			$form.ajaxSubmit({
				success: function(response) {
					hideModal();
					$form.get(0).reset();
					showModal('.thanks2-modal');
					setTimeout('hideModal()',3000);
					if ($('body').data('metrika') && window['yaCounter'+$('body').data('metrika')]) window['yaCounter'+$('body').data('metrika')].reachGoal('request');
					if (window._gaq) _gaq.push(['_trackEvent','Form','Sent']);
				}
			});
		}
	});

	/*$('.send-btn').click(function(e) {
		e.preventDefault();
		$(this).closest('form').submit();
	});*/

	$('.call-request-btn').click(function(e) {
		e.preventDefault();
		var mod = $('.modal.recall-modal');
		if (mod.hasClass('open')) {
			hideModal(mod);
			mod.removeClass('open');
		} else {
			showModal(mod);
			mod.addClass('open');
		}
	});


	$('.overlay, .modal .close-btn, .modal .modal-close-text').click(function(e) {
		e.preventDefault();
		hideModal('.modal');
		$('.modal').removeClass('open');
	});

});

function showModal(modal) {
	showModalTop(modal, 66);
}

function showModalTop(modal, top) {
	modal = modal instanceof jQuery ? modal : jQuery(modal);
	modal.css("top", top + "px");
	modal.show();
	$(".overlay").show();
}

function hideModal() {
	$(".modals > div").hide();
	$(".overlay").hide();
}

function dif(diffSecs) {
	var sec = Math.floor(diffSecs % 60);
	var mins = Math.floor(diffSecs / 60) % 60;
	var hours = Math.floor(diffSecs / 60 / 60) % 24;
	var days = Math.floor(diffSecs / 60 / 60 / 24);

	var temp = convert(days, ['день', 'дня', 'дней']);
	$('.timer .days').html(temp[0]);
	$('.timer .days-text').html(temp[1]);
	var temp = convert(hours, ['час', 'часа', 'часов']);
	$('.timer .hours').html(temp[0] < 10 ? '0' + temp[0] : temp[0]);
	$('.timer .hours-text').html(temp[1]);
	var temp = convert(mins, ['минута', 'минуты', 'минут']);
	$('.timer .mins').html(temp[0] < 10 ? '0' + temp[0] : temp[0]);
	$('.timer .mins-text').html(temp[1]);
	var temp = convert(sec, ['секунда', 'секунды', 'секунд']);
	$('.timer .secs').html(temp[0] < 10 ? '0' + temp[0] : temp[0]);
	$('.timer .secs-text').html(temp[1]);

	if (diffSecs > 0)
	{
		t = setTimeout(function() {
			dif(diffSecs - 1)
		}, 1000);
	}
}

function convert(n, ar) {
	var o = n % 10;
	var l, g;
	switch (o) {
		case 1:
			l = 0;
			break;
		case 2:
		case 3:
		case 4:
			l = 1;
			break;
		default:
			l = 2;
			break;
	}

	var g = n % 100;
	if (g == 10 || g == 11 || g == 12 || g == 13 || g == 14) {
		l = 2;
	}
	return [n, ar[l]];
}

function checkEmail(mail) {
	return /^[a-zа-я0-9,!#\$%&'\*\+/=\?\^_`\{\|}~-]+(\.[a-zа-я0-9,!#\$%&'\*\+/=\?\^_`\{\|}~-]+)*@[a-zа-я0-9-]+(\.[a-z0-9-]+)*\.([a-zрф]{2,})$/i.test(mail);
}