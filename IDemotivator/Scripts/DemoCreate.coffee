﻿# CoffeeScript
canvas = new fabric.Canvas("canvas")

canvas.cli
string1 = new fabric.IText("Top text",
  fontFamily: "Georgia"
  originX: 'center'
  left:300
  selectable: false
  z_index: 1
  fontSize: 50
  lockScalingX: true
  lockScalingY: true
  editable: false
  hasRotatingPoint: false
  transparentCorners: false

)

string2 = new fabric.IText("Bott text",
  fontFamily: "Georgia"
  originX: "centre"
  top: 290
  left:300
  selectable: false
  z_index: 1
  fontSize: 50
  lockScalingX: true
  lockScalingY: true
  editable: false
  hasRotatingPoint: false
  transparentCorners: false
)
canvas.add string1
canvas.add string2
canvas.renderAll()

document.getElementById('str22').oninput = ->
    #canvas.setActiveObject(string2);
    tstring = ""
    tsring = document.getElementById('str22').value
    string2.set('text',tsring)
    canvas.renderAll()

document.getElementById('str11').oninput = ->
    #canvas.setActiveObject(string1);
    tstring = ""
    tsring = document.getElementById('str11').value
    string1.set('text',tsring)
    canvas.renderAll()


document.getElementById('create').onclick = ->
  JSONstring = JSON.stringify(canvas)
  JSONstring = JSONstring.replace("data:image/png;base64,", "")
  document.getElementById("JSON").value = JSONstring

$('#btnFileUpload').click ->
  file = undefined
  formData = undefined
  imgsrc = undefined
  formData = new FormData
  file = document.getElementById('fileInput').files[0]
  formData.append 'fileInput', file
  imgsrc = file

  $.ajax
    url: '/Demotivators/Upload'
    type: 'POST'
    data: formData
    contentType: false
    processData: false
    success: (data) ->
      document.getElementById('Url_Img_Origin').setAttribute 'value', data.Uri
      alert data.Uri
      fabric.Image.fromURL data.Uri, (oImg) ->
        canvas.setBackgroundImage oImg
        canvas.backgroundImage.width = canvas.getWidth() - 10
        canvas.backgroundImage.height = canvas.getHeight() - 10
        canvas.renderAll()
      return
  return




#document.getElementById('str11').onclick = ->
#  tempObj = canvasFabric.getActiveObject()
 # srcObj = tempObj.getSrc()
#  idexEl = srcObj.indefOf("google")
 # if indexEL>-1



#$('#btnFileUpload').click ->
 # formData = new FormData
 # file = document.getElementById('fileInput').files[0]
 # formData.append 'fileInput', file
 # imgsrc = file.value
 
  #$.ajax
  #  url: '/Demotivators/Upload'
 #   type: 'POST'
  #  data: formData
 #   contentType: false
 #   processData: false
 #   success: (data) ->
 #     document.getElementById('Url_Img_Origin').setAttribute 'value', data.Uri
  #    alert data.Uri
  #    return
#  return