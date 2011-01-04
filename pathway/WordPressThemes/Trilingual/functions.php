<?php
if ( function_exists('register_sidebar') ){
    register_sidebar(array(
        'name' => 'leftsidebar',
        'before_widget' => '',
        'after_widget' => '',
        'before_title' => '<h4>',
        'after_title' => '</h4>',
    ));

register_sidebar(array(
        'name' => 'rightsidebar',
        'before_widget' => '',
        'after_widget' => '',
        'before_title' => '<h4>',
        'after_title' => '</h4>',
    ));
}
?>