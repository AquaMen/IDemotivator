# CoffeeScript

fileIMG  = undefined
URLorigin= undefined
URLimge  = undefined
sizeH    = undefined
sizeW    = undefined
sizeImgH = undefined
sizeImgW = undefined
posImgL  = undefined
posImgT  = undefined
rect     = undefined   
Heigh = window.innerHeight
IMG = undefined


#start init canvas container
sizeH = (Heigh / 100) * 80
sizeW = sizeH * 3 / 4 
sizeImgH = sizeW - 100
sizeImgW = sizeH - 220
posImgL  = 50
posImgT  = 70
$('#canvas-container').height( sizeH)
$('#canvas-container').width( sizeW)

#start init fabrick js
canvas   = new fabric.StaticCanvas("canvas")
canvas.cli
canvas.backgroundColor="balck";
canvas.setHeight(sizeH);
canvas.setWidth(sizeW);

rect = new (fabric.Rect)(
  fill: 'black'
  left :posImgL
  top : posImgT
  width: sizeImgW
  height: sizeImgH
  strokeWidth: 8
  stroke: 'white' 
  selectable: false
  )

string1 = new fabric.IText("Top text",
  fontFamily: "Georgia white"
  fill:'white'
  left: sizeW / 2
  originX: "centre"
  selectable: false
  fontSize: 50
  editable: false
  hasRotatingPoint: false
  transparentCorners: false
)

string2 = new fabric.IText("Bott text",
  top: sizeH - 130
  fontFamily: "Georgia white"
  fill:'white'
  left: sizeW / 2
  originX: "centre"
  selectable: false
  fontSize: 50
  editable: false
  hasRotatingPoint: false
  transparentCorners: false
)
canvas.add string1
canvas.add string2
canvas.add rect
canvas.renderAll()


patV =->
  sizeW = (Heigh / 100) * 90
  sizeH = sizeW * 12 / 16
  sizeImgW = sizeW - 140
  sizeImgH = sizeH - 170
  posImgL = 70
  posImgT = 70
  $('#canvas-container').height( sizeH)
  $('#canvas-container').width( sizeW)
  $('#canvas-container').css( { marginTop : "100px"} );
  string1.left =sizeW / 2
  string2.left =sizeW / 2
  string2.top  =sizeH - 90
  canvas.setHeight(sizeH);
  canvas.setWidth(sizeW);
  rect.top   = 70
  rect.left  = 70
  rect.width = sizeW - 140
  rect.height= sizeH - 170
  if IMG != undefined
    IMG.set('left',posImgL + 5);
    IMG.set('top', posImgT + 5);
    IMG.width = sizeImgW
    IMG.height = sizeImgH
  canvas.renderAll()

patH =->
  sizeH = (Heigh / 100) * 80
  sizeW = sizeH * 3 / 4 
  sizeImgW = sizeW - 100
  sizeImgH = sizeH - 220
  posImgL = 50
  posImgT = 70
  $('#canvas-container').height( sizeH)
  $('#canvas-container').width( sizeW)
  $('#canvas-container').css( { marginTop : "60px"} );
  string1.left =sizeW / 2
  string2.left =sizeW / 2
  string2.top =sizeH - 130
  canvas.setHeight(sizeH);
  canvas.setWidth(sizeW);
  rect.top   = 70
  rect.left  = 50
  rect.width = sizeW - 100
  rect.height= sizeH - 220
  if IMG != undefined
    IMG.set('left',posImgL + 5);
    IMG.set('top', posImgT + 5);
    IMG.width = sizeImgW
    IMG.height = sizeImgH
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

document.getElementById('Hpattern').onclick = ->
  t = document.getElementById('Hpattern')
  if !t.classList.contains('active')
    patH()
    el = document.getElementById('Wpattern')
    el.classList.remove 'active'
    t.classList.add 'active'
  return

document.getElementById('Wpattern').onclick = ->
  t = document.getElementById('Wpattern')
  if !t.classList.contains('active')
    patV()
    el = document.getElementById('Hpattern')
    el.classList.remove 'active'
    t.classList.add 'active'
  return

document.getElementById('create').onclick = ->
  formData = new FormData
  formData.append 'fileInput', fileIMG
  demotIMG = document.getElementById('canvas').toDataURL();
  formData.append 'canvas', demotIMG
  $.ajax
    async: false
    url: '/Demotivators/Upload'
    type: 'POST'
    data: formData
    contentType: false
    processData: false
    success: (data) ->
      document.getElementById('Url_Img_Origin').setAttribute 'value', data[0].Uri
      document.getElementById('Url_Img').setAttribute 'value', data[1].Uri
      canvas.setBackgroundImage data[0].Uri, canvas.renderAll.bind(canvas),
          originX: 'left'
          originY: 'top'
      return
  JSONstring = JSON.stringify(canvas)
  JSONstring = JSONstring.replace("data:image/png;base64,", "")
  document.getElementById("JSON").value = JSONstring
  return

$('#btnFileUpload').click ->
  fileIMG = document.getElementById('fileInput').files[0]
  url = URL.createObjectURL(fileIMG)
  fabric.Image.fromURL url, (oImg) ->
    scaleX:  oImg.width = sizeImgW
    scaleY:  oImg.height = sizeImgH
    
    oImg.set('left',posImgL + 5);
    oImg.set('top', posImgT + 5);
    if IMG != undefined
      canvas.remove(IMG)
    canvas.add oImg
    IMG = oImg
    canvas.renderAll()
  return



  #fabric.Image.fromURL url, (oImg) ->
  #  left :100
   # scaleX:  oImg.width = sizeImgW
  #  scaleY:  oImg.height = sizeImgH
  #  canvas.add(oImg)
   # canvas.renderAll()
  return




#$('#btnFileUpload').click ->
#  file = undefined
#  imgsrc = undefined
#  fileIMG = document.getElementById('fileInput').files[0]
#  fr = new FileReader
#  fr.onloadend = fileIMG
#  fr.readAsDataURL(file);
#  urlIMG = fileIMG.getAsDataURL();
#  fabric.Image.fromURL urlIMG, (oImg) ->
#        canvas.setBackgroundImage oImg
#        canvas.backgroundImage.width = canvas.getWidth() - 10
#        canvas.backgroundImage.height = canvas.getHeight() - 10
#        canvas.renderAll()


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