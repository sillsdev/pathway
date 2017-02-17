	function search(){
		var key = $('.form-control').val();
		if (key.length > 2){
			$('.s').each(function(i, obj){
				var cur = $(this).text();
				if (cur.indexOf(key)!=-1){
					$(this)[0].click();
					return false;
				}
			});
		}
	}
