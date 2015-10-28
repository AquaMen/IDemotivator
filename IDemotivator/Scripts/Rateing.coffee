# CoffeeScript
clickedFlag = false
$(".ratingStar").mouseover ->
  $(this).attr("src", "/images/yellowstar.gif").prevAll("img.ratingStar").attr "src", "/images/yellowstar.gif"

$(".ratingStar, #radingDiv").mouseout ->
  $(this).attr "src", "/images/whitestar.gif"

$("#ratingDiv").mouseout ->
  $(".ratingStar").attr "src", "/images/whitestar.gif"  unless clickedFlag

$(".ratingStar").click ->
  clickedFlag = true
  $(".ratingStar").unbind("mouseout mouseover click").css "cursor", "default"
  url = "/Home/SendRating?r=" + $(this).attr("data-value") + "&s=5&id=@Model&url=@url"
  $.post url, null, (data) ->
    $("#lblResult").html data


$(document).ready ->
  clickedFlag = true
  $(".ratingStar").unbind("mouseout mouseover click").css "cursor", "default"
  url = "/Home/SendRating?r=" + $(this).attr("data-value") + "&s=5&id=@Model&url=@url"
  $.post url, null, (data) ->
    $("#lblResult").html data


$("#lblResult").ajaxStart ->
  $("#lblResult").html "Processing ...."

$("#lblResult").ajaxError ->
  $("#lblResult").html "<br />Error occured."
