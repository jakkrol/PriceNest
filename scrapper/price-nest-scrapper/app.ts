import { chromium } from 'playwright';
import scrapeCeneo from './scrapper.js';
// (async () => {
//   console.log("Init...");
//   const browser = await chromium.launch();
//   const page = await browser.newPage();
  
  
//   const url = 'https://www.scrapethissite.com/pages/simple/'; 
  
//   await page.goto(url);
//   const title = await page.title();
  
//   console.log(`Connected: ${title}`);
  
//   await browser.close();
// })();

scrapeCeneo("laptop");