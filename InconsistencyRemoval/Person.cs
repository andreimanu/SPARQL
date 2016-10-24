using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;

namespace InconsistencyRemoval {
    public class Person {
        public Dictionary<INode, SparqlResult> People;
        private SparqlRemoteEndpoint endpoint;

        public string Name { get; set; }
        public string Occupation { get; set; }

        public Person(string Name, string Occupation) {
            this.Name = Name;
            this.Occupation = Occupation;
        }

        public IDictionary<INode, SparqlResult> Query1() {
            StringBuilder sbQuery = new StringBuilder();
            RunQuery();
            return People;
        }

        private void RunQuery() {
            string Query = "SELECT ?person ?name ?surname ?givenName ?abstract (COUNT(?external) AS ?externalCount)  (COUNT(?abstract) AS ?abstractCount) " +
                            "FROM <http://dbpedia.org> " +
                            "WHERE { " +
                            "    ?person a foaf:Person . " +
                            "    ?person foaf:name ?name . " +
                            "    ?person dbo:abstract ?abstract .  " +
                            "    OPTIONAL { " +
                            "    ?person foaf:surname ?surname . " +
                            "    ?person foaf:givenName ?givenName .  " +
                            "    ?person dbo:wikiPageExternalLink ?external . " +
                            "    } " +
                            "    FILTER(lang(?abstract) = \'en\') ";

            string[] split = Name.Split(' ');
            foreach(string part in split) {
                Query += "FILTER(regex(?name, \"" + part.Trim() + "\", \"i\")) ";
                Query += "FILTER(regex(?abstract, \"" + part.Trim() + "\", \"i\")) ";
            }
            Query +=     "} " +
                         "ORDER BY DESC(?externalCount) DESC(?abstractCount) ";

            var result = endpoint.QueryWithResultSet(Query);
            foreach (var i in result) {
                tryAddPerson(i);
            }

        }

        public SparqlResult RunQuery2() {
            string Query = "SELECT ?person ?name ?surname ?givenName ?abstract (COUNT(?external) AS ?externalCount)  (COUNT(?abstract) AS ?abstractCount) " +
                            "FROM <http://dbpedia.org> " +
                            "WHERE { " +
                            "    ?person a foaf:Person . " +
                            "    ?person foaf:name ?name . " +
                            "    ?person dbo:abstract ?abstract .  " +
                            "    OPTIONAL { " +
                            "    ?person foaf:surname ?surname . " +
                            "    ?person foaf:givenName ?givenName .  " +
                            "    ?person dbo:wikiPageExternalLink ?external . " +
                            "    } " +
                            "    FILTER(lang(?abstract) = \'en\') ";

            string[] split = Name.Split(' ');
            foreach (string part in split) {
                Query += "FILTER(regex(?name, \"" + part.Trim() + "\", \"i\")) ";
                Query += "FILTER(regex(?abstract, \"" + part.Trim() + "\", \"i\")) ";
            }
            Query +=     "} " +
                         "ORDER BY DESC(?externalCount) DESC(?abstractCount) " +
                         "LIMIT 1";

            var result = endpoint.QueryWithResultSet(Query);
            if (result.Count > 0)
                return result[0];
            else
                return null;
        }

        private void tryAddPerson(SparqlResult i) {
            if (!People.ContainsKey(i.Value("person")))
                People.Add(i.Value("person"), i);
        }

        public void Load() {
            endpoint = new SparqlRemoteEndpoint(new Uri("http://localhost:8890/sparql"));
            People = new Dictionary<INode, SparqlResult>();
        }

        
    }
}
