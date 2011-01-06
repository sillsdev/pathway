<?php get_header(); ?>
  <div id="bd" class="yui-navset">
		    
    <div id="yui-main">
    
		<div class="yui-b" >
			<div class="yui-ge"><div class="yui-u first">

	<?php 
	require("searchform.php");
	?>

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