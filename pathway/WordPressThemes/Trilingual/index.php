<?php get_header(); ?>
  <div id="bd" class="yui-navset">
		    
    <div id="yui-main">
    
		<div class="yui-b" >
			<div class="yui-ge"><div class="yui-u first">

		   		<table>	
		   		<tr> 
		   			<td>
						<h3>Look up English, Chinese or Nuosu Yi</h3>
						<br>
						&nbsp;
				   		<a href="">Yi Index Chart</a>				   		
						&nbsp;&nbsp;
				   		<a href="">Radical Stroke Index</a>
		   			</td>

				</tr>
		         <tr>
		   			<td>
				 	     <form id="searchform" method="get" action="<?php bloginfo('home'); ?>">
							<input type="text" name="s" id="s" value="" size=40>
							<input type="submit" id="searchsubmit" name="search" value="Search" />
			 	     	 </form>		   			
		   			</td>
		   		</tr>
		   		</table>  	


<!-- 2nd sidebar -->
</div><!-- end yiu-u -->
<div class="yui-u" id="third"><?php do_action('icl_language_selector'); ?></div>


<div class="yui-u" id="third"><?php if ( !function_exists('dynamic_sidebar') || !dynamic_sidebar('rightsidebar') ) : ?><h4>Extra Column</h4><p>You can fill this column by editing the index.php theme file. Or by Widget support.</p><?php endif; ?></div>
 

<!-- end 2nd sidebar -->
			</div>
		</div>
	</div>
	<div class="yui-b" id="secondary">

<?php get_sidebar(); ?>

	</div>
	
  </div>
<?php get_footer(); ?>