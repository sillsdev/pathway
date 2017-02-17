	function search(){
		var key = $('.form-control').val();
		var results = $('#searchResults')[0];
		if (key.length > 2){
			$('.s').each(function(i, obj){
				var cur = $(this).text();
				if (cur.indexOf(key)!=-1){
					var li = document.createElement("li");
					li.innerHTML = $(this)[0].outerHTML;
					results.appendChild(li);
				}
			});
			$('#resultsBtn')[0].click()
		}
	}
