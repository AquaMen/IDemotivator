# CoffeeScript
canvas = new fabric.Canvas("canvas")
$(document).ready ->
  resu = document.getElementById("JSON").value
  canvas.loadFromJSON resu
  canvas.renderAll()