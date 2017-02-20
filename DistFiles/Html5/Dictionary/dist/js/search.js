	function search(){
		var key = $('.form-control').val();
		var results = $('#searchResults')[0];
		while (results.firstChild){
			results.removeChild(results.firstChild);
		}
		if (key.length > 2){
			$('.s').each(function(i, obj){
				var cur = $(this).text();
				if (cur.indexOf(key)!=-1){
					var li = document.createElement("li");
					li.innerHTML = $(this)[0].outerHTML;
					results.appendChild(li);
				}
			});
			if (results.childNodes.length == 1){
				$('#searchResults a')[0].click();
			} else {
				$('#resultsBtn')[0].click();
			}
		}
	}
