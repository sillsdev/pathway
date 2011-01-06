		   		<table>	
		   		<tr> 
		   			<td><h3>Look up English, Chinese or Nuosu Yi</h3>
						<br>
						&nbsp;
				   		<a href=""><?php _e('[:en]Yi Index Chart[:zh]&#24413;&#25991;&#38899;&#24207;&#26816;&#23383;&#34920;');?></a>				   		
						&nbsp;&nbsp;
				   		<a href=""><?php _e('[:en]Radical Stroke Index[:zh]&#24413;&#25991;&#37096;&#39318;&#26816;&#23383;&#34920;');?></a>
		   			</td>

				</tr>
		         <tr>
		   			<td>
				 	     <form id="searchform" method="get" action="<?php bloginfo('url'); ?>"> 
							<input type="text" name="s" id="s" value="<?php the_search_query(); ?>" size=40>
							<input type="submit" id="searchsubmit" name="search" value="Search" />
							<select name="key">
								<option value=""></option>
								<option value="1">Nuosu Character</option>
								<option value="2">Chinese Character</option>
								<option value="3">English</option>
							</select>							
			 	     	 </form>
		   			</td>
		   		</tr>
		   		</table>  	
