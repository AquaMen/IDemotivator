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
IMG = undefined

Heigh = window.innerHeight
if Heigh< 300
  Heigh = 300

#start init canvas container
sizeH = (Heigh / 100) * 80
sizeW = sizeH * 3 / 4 
sizeImgH = sizeH - (sizeH / 4)
tmpSizez = (sizeH / 20)
sizeImgW = sizeW - tmpSizez*2.3
posImgL  = tmpSizez
posImgT  = sizeH / 30
$('#canvas-container').height( sizeH )
$('#canvas-container').width ( sizeW )

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
  strokeWidth: 10
  stroke: 'white' 
  selectable: false
  )

string1 = new fabric.IText("Top text",
  fontFamily: "Georgia white"
  fill:'white'
  left: sizeW / 2
  top:(sizeH/100)*88
  originX: "centre"
  selectable: false
  fontSize: 25
  editable: false
  hasRotatingPoint: false
  transparentCorners: false
)

string2 = new fabric.IText("Bott text",
  top: (sizeH/100)*80
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
  sizeImgH = sizeH - (sizeH / 4)
  tmpSizez = (sizeH / 20)
  sizeImgW = sizeW - tmpSizez*2.3
  posImgL  = tmpSizez
  posImgT  = sizeH / 30
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
  $.blockUI css:
    border: 'none'
    padding: '15px'
    backgroundColor: '#000'
    '-webkit-border-radius': '10px'
    '-moz-border-radius': '10px'
    opacity: .5
    color: '#fff'
  formData = new FormData
  formData.append 'fileInput', fileIMG
  demotIMG = document.getElementById('canvas').toDataURL();
  JSONstring = JSON.stringify(canvas)
  document.getElementById("JSON").value = JSONstring.replace("data:image/png;base64,", "")
  formData.append 'canvas', demotIMG
  $.ajax
    async: false
    url: '/Demotivators/Upload'
    type: 'POST'
    data: formData
    contentType: false
    processData: false
    success: (data) ->
      document.getElementById('Url_Img_Origin').setAttribute 'value', data[0]
      document.getElementById('Url_Img').setAttribute 'value', data[1]
      return
  return

$('#btnFileUpload').click ->
  fileIMG = document.getElementById('fileInput').files[0]
  url = URL.createObjectURL(fileIMG)
  document.getElementById('create').disabled = false
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