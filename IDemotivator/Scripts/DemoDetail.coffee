# CoffeeScript
canvas = new fabric.Canvas("canvas")
$(document).ready ->
  resu = document.getElementById("url2").value
  canvas.loadFromJSON resu
  canvas.renderAll()