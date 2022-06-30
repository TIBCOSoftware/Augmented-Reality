
const authorization = "Bearer CIC~aV4OL21PcLY-2y-TS0dqf2Fz";

function requestOptions(authorization) {
  var myHeaders = new Headers();
  myHeaders.append("Authorization", authorization);

  var requestOptions = {
    method: 'GET',
    headers: myHeaders,
    redirect: 'follow'
  };
  return requestOptions;
}

async function getApplicationInfo(sandboxId,applicationName, authorization) {
// https://liveapps.cloud.tibco.com
  return fetch(
    `/case/v1/types?$sandbox=${sandboxId}&$filter=applicationName eq '${applicationName}' &$select=b,ac,c,s`,
    requestOptions(authorization)
  ).then(response => response.text())
  .then(result =>  JSON.parse(result));
}

async function getCases(sandboxId,applicationId,search,authorization) {
  return fetch(
    `/case/cases?$sandbox=${sandboxId}&$filter=applicationId eq ${applicationId} and typeId eq 1&$select=c,cr&$top=100&$search=${search}`,
    requestOptions(authorization))
    .then(response => response.text())
    .then(result =>  JSON.parse(result))

  }
async function getArtifacts(sandboxId,caseRef,authorization) {

  return fetch(`webresource/v1/caseFolders/${caseRef}/artifacts/?$sandbox=${sandboxId}`,
    requestOptions(authorization)
  ).then(response => response.text())
  .then(result =>  {
    console.log(result);
    const list = JSON.parse(result);
    return list;
  })
  .then(list => fetch(`/webresource/folders/${caseRef}/${sandboxId}/${list[0].name}`,
    requestOptions(authorization)
  ))
  .then(response => response.blob())
  .catch(error => console.log('error', error));
}

AFRAME.registerComponent('tibco-case', {
  schema: {
    casedata: {type: 'string'},
    caseReference: {type: 'string'},
    sandboxId: {type: 'string'}
  },
  init: function () {
    this.case = JSON.parse(this.data.casedata);
    //this.caseReference = data.caseReference;
    this.width = 0.8;
    this.height = 1.8;
    var slate = document.createElement('a-entity');
    slate.setAttribute('geometry', `primitive: plane;  width: ${this.width}; height: ${this.height}`);
    slate.setAttribute('material',"color: #444444; transparent: true; opacity: 0.80;");
    // parent origin will be left bottom corner
    slate.object3D.position.x = this.width/2;
    slate.object3D.position.y = this.height/2;
    this.el.appendChild(slate);
    // add text box
    var textbox = document.createElement('a-entity');
    textbox.setAttribute('text',`anchor: left; baseline: top; wrapCount: 35; \
    transparent: true; \
    opacity: 1.00; \
    width: ${0.9 * this.width} \
    color: #8888FF; \
    value: 1 \n 2 \n 3 \n 4`);
    let text = "Name :" + this.case.Name + "\n" +
    "Description :" + this.case.Description + "\n";
    textbox.object3D.position.z = 0.005;
    textbox.object3D.position.x = 0.05 * this.width;
    textbox.object3D.position.y = this.height-0.05*this.width;
    textbox.setAttribute( "text", "value", text );
    this.el.appendChild(textbox);
    this.setPicture();

  },
  setPicture: function() {
    this.reader = new FileReader();
    this.reader.onloadend = this.onloadend.bind(this); // else this would be the reader
    getArtifacts(this.data.sandboxId,this.data.caseReference,authorization)
    .then(imageBlob => this.reader.readAsDataURL(imageBlob))
    .catch(error => console.log('error', error));
  },

  onloadend: function() {
     const base64data = this.reader.result;
  var photo = document.createElement('a-image');
  photo.setAttribute('geometry', `primitive: plane;  width: ${0.9*this.width}; height: ${0.9*this.width}`);
  photo.setAttribute('src',base64data);
  photo.object3D.position.z = 0.005;
  photo.object3D.position.x = this.width/2;
  photo.object3D.position.y = this.height/4;
  this.el.appendChild(photo);
  }
});

  AFRAME.registerComponent('tibco-liveapps', {
    schema: {
      applicationName: {type: 'string'},
      search: {type:'string'}
    },
    init: function () {
      var myHeaders = new Headers();
      myHeaders.append("Authorization", authorization);
      this.cases = [];
      var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
      };
      fetch("/organisation/v1/sandboxes?$filter=type eq Production", requestOptions)
      .then(response => response.text())
      .then(result => {
        this.sandboxId = JSON.parse(result)[0].id;
        return getApplicationInfo(this.sandboxId, this.data.applicationName, authorization);
      })
      .then(info => getCases(this.sandboxId,info[0].applicationId,this.data.search,authorization))
      .then(data => {this.cases = data; this.displayCases()})
      .catch(error => console.log('error', error));
    },
    displayCases() {
      this.cases.forEach((item, i) => {
        const card = JSON.parse(item.casedata);
        console.log(`adding card for case ${item}`);

          var entity = document.createElement('a-entity');
          entity.object3D.position.x = i*1;
          entity.setAttribute("tibco-case", {
            "casedata": item.casedata,
            "caseReference": item.caseReference,
            "sandboxId": this.sandboxId,
          });
          this.el.appendChild(entity);

      })

    }
  })
