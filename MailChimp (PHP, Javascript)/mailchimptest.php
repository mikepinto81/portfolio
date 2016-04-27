<?php 
/**
 * Author: Mike Pinto (mike@zotnip.com)
 * Submit form to Mailchimp API
 */
require 'mailchimp.php'; 

//new mailchimp object
$mailChimp = new MailChimp('ENTERAPIKEY');

//get the available lists 
//lists = $mailChimp->get('lists');

//testing the second list from returned lists
//$testList = $lists["lists"][1];

//store the id of the list
//$listID = $testList["id"];
$listID = "LISTID";

//setup variables
$FNAME = "";
$LNAME = "";
$ZIP = "";
$EMAIL = "";

//get variables from POST
if(isset($_POST['FNAME']))	
	$FNAME = $_POST["FNAME"];
if(isset($_POST['LNAME']))
	$LNAME = $_POST["LNAME"];
if(isset($_POST['ZIP']))
	$ZIP = $_POST["ZIP"];
if(isset($_POST['EMAIL']))
	$EMAIL = $_POST["EMAIL"];

//send info to mailchimp and get response
$result = $mailChimp->post('lists/'.$listID.'/members', array(
	'email_address' => $EMAIL,
	'status' => 'subscribed',
	'merge_fields' => array('FNAME' => $FNAME, 'LNAME' => $LNAME, $ZIP => '08088')
));

//if status is 400 echo the details of the error
if($result["status"] == "400")
{	
	echo $result["detail"] . '<br />';
} 
//otherwise just show the status
else { 
	//echo the status
	echo $result["status"];
}

//show available options
//print_r($result);
?>