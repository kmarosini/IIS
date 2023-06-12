using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Commons.Xml.Relaxng;

namespace IIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        
        public bool ProcessXmlFileWithXSD(IFormFile file)
        {
            // Spremanje datoteke na privremeno mjesto
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            var solutionDirectoryPath = FindSolutionPath();
            string xmlFilePath = Path.Combine(solutionDirectoryPath, "FootballPlayers.xml");
            string xsdFilePath = Path.Combine(solutionDirectoryPath, "FootballPlayer.xsd");
            Console.WriteLine(xmlFilePath);
            // Učitavanje XSD datoteke
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(xsdFilePath));

            // Učitavanje XML datoteke
            XmlDocument doc = new XmlDocument();
            doc.Load(file.OpenReadStream());

            // Validacija
            string msg = "";
            doc.Schemas = schemas;
            doc.Validate((sender, args) =>
            {
                msg = args.Message;
            });

            if (string.IsNullOrEmpty(msg))
            {
                doc.Save(xmlFilePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ProcessXmlFileWithRNG(IFormFile file)
        {
            // Saving the file to a temporary location
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            var solutionDirectoryPath = FindSolutionPath();
            string xmlFilePath = Path.Combine(solutionDirectoryPath, "FootballPlayers.xml");
            string rngFilePath = Path.Combine(solutionDirectoryPath, "FootballPlayerRNG.rng");

            // Loading the RNG file
            RelaxngPattern rng;
            using (var rngReader = XmlReader.Create(rngFilePath))
            {
                rng = RelaxngPattern.Read(rngReader);
            }

            // Loading the XML file
            XmlDocument doc = new XmlDocument();
            doc.Load(file.OpenReadStream());

            // Validation
            string msg = "";
            using (var xmlReader = new RelaxngValidatingReader(new XmlTextReader(xmlFilePath), rng))
            {
                try
                {
                    while (xmlReader.Read()) { } // Exception if invalid
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }

            if (string.IsNullOrEmpty(msg))
            {
                doc.Save(xmlFilePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost(Name = "SaveWithXSD")]
        public IActionResult SaveWithXSD(IFormFile file)
        {
            try
            {
                bool isValid = ProcessXmlFileWithXSD(file);

                if (isValid)
                {
                    return Ok("XML file is valid according to the provided XSD schema.");
                }
                else
                {
                    return BadRequest("XML file is not valid according to the provided XSD schema.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost(Name = "SaveWithRNG")]
        public IActionResult SaveWithRNG(IFormFile file)
        {
            try
            {
                bool isValid = ProcessXmlFileWithRNG(file);

                if (isValid)
                {
                    return Ok("XML file is valid according to the provided RNG schema.");
                }
                else
                {
                    return BadRequest("XML file is not valid according to the provided RNG schema.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
        public string FindSolutionPath()
        {
            string solutionDirectoryPath = AppDomain.CurrentDomain.BaseDirectory; // Gets the bin/debug directory path
            for (int i = 0; i < 5; i++)
            {
                var directoryInfo = Directory.GetParent(solutionDirectoryPath);
                if (directoryInfo != null)
                {
                    solutionDirectoryPath = directoryInfo.FullName;
                }
                else
                {
                    throw new Exception($"Could not find parent directory at level {i}");
                }
            }
            return solutionDirectoryPath;
        }
    }

    
}
