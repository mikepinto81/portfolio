<?php
defined( 'ABSPATH' ) or die( 'No script kiddies please!' );
/*
Plugin Name: UFCWWest ScoreCards ShortCode
Description: usage [ShowScorecard years="2013 2014 2015"] (years sep by space)
Version: 1
*/

/**
* Load shortcode php file
*/
require (plugin_dir_path(__FILE__) . 'scorecardshortcode.php');


//register javascript files
function scorecardsshortcodescripts()
{
	wp_enqueue_script( 'jquery' );

    wp_register_script( 'scorecardshortcodeJS', plugins_url( '/scorecardshortcodeJS.js', __FILE__ ) );  
    wp_enqueue_script( 'scorecardshortcodeJS' );
	
	//load datatables from datatables cdn
	wp_enqueue_script('datatables', "https://cdn.datatables.net/r/dt/dt-1.10.9,b-1.0.3,cr-1.2.0,r-1.0.7,sc-1.3.0/datatables.min.js", array(), '1', false);
	
}
add_action( 'wp_enqueue_scripts', 'scorecardsshortcodescripts' );
?>